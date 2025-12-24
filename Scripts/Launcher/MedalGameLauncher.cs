using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MedalGame
{
    public interface IMedalGameSceneInitializer
    {
        void Initialize(MedalGameOperation operation);
    }

    public interface IMedalGameLoadCompleteReceiver
    {
        void OnMedalGameLoaded(MedalGameReferenceHub hub);
    }

    public class MedalGameOperation
    {
        public int InitialMedalCount { get; set; } = 10;
    }

    public class MedalGameSceneResult
    {
        public int SceneID { get; private set; }
        public Scene Scene { get; private set; }

        public void SetSceneId(int id)
        {
            SceneID = id;
            Scene = SceneManager.GetSceneAt(id);
        }

        public AsyncOperation LoadOperation { get; set; }
        public bool AllowSceneActivation
        {
            get => LoadOperation.allowSceneActivation;
            set => LoadOperation.allowSceneActivation = value;
        }

        const float COMPLETION_THRESHOLD = 0.9f;
        public bool IsPrepared => LoadOperation.progress >= COMPLETION_THRESHOLD;

        public event Action<AsyncOperation> OnPrepared;
        public void InvokeOnPrepared(AsyncOperation asyncOperation)
        {
            if (!IsPrepared) return;

            OnPrepared?.Invoke(asyncOperation);
            OnPrepared = null;
        }

        public void Activate(Action onActivated = null)
        {
            if (IsPrepared)
            {
                AllowSceneActivation = true;
                onActivated?.Invoke();
            }
            else
            {
                OnPrepared += _ =>
                {
                    AllowSceneActivation = true;
                    onActivated?.Invoke();
                };
            }
        }
    }

    public class MedalGameLauncher : MonoBehaviour
    {
        const string SCENE_NAME = "MedalGame";
        readonly List<MedalGameSceneResult> _activeResults = new();

        public MedalGameSceneResult LaunchAsync(MedalGameOperation operation)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(SCENE_NAME, LoadSceneMode.Additive);
            asyncOperation.allowSceneActivation = false;

            var result = new MedalGameSceneResult
            {
                LoadOperation = asyncOperation,
            };

            asyncOperation.completed += _ => StartCoroutine(InitializeSceneAfterActivation(operation, result));

            _activeResults.Add(result);
            return result;
        }

        private IEnumerator InitializeSceneAfterActivation(MedalGameOperation operation, MedalGameSceneResult result)
        {
            yield return null;

            var index = SceneManager.sceneCount - 1;
            result.SetSceneId(index);

            var scene = result.Scene;
            if (!scene.IsValid())
            {
                Debug.LogError($"Scene not found!");
                yield break;
            }

            var rootObjects = scene.GetRootGameObjects();

            MedalGameReferenceHub hub = null;
            foreach (var rootObject in rootObjects)
            {
                hub = rootObject.GetComponentInChildren<MedalGameReferenceHub>(true);
                if (hub != null) break;
            }

            if (hub == null)
            {
                Debug.LogError("MedalGameReferenceHub not found in scene!");
                yield break;
            }

            foreach (var rootObject in rootObjects)
            {
                var initializers = rootObject.GetComponentsInChildren<IMedalGameSceneInitializer>(true);
                foreach (var initializer in initializers)
                {
                    initializer.Initialize(operation);
                }
            }

            foreach (var rootObject in rootObjects)
            {
                var receivers = rootObject.GetComponentsInChildren<IMedalGameLoadCompleteReceiver>(true);
                foreach (var receiver in receivers)
                {
                    receiver.OnMedalGameLoaded(hub);
                }
            }
        }

        public static MedalGameLauncher Instance { get; private set; }

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            foreach (var result in _activeResults)
            {
                if (result.IsPrepared)
                {
                    result.InvokeOnPrepared(result.LoadOperation);
                }
            }
        }

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}
