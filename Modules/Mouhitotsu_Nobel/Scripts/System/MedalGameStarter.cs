using System.Collections;
using MedalGame;
using UnityEngine;

namespace MantenseiNobel.Mouhitotsu
{
    public interface IMedalGameSceneLoadNotifier
    {
        void OnMedalGameSceneActivate(MedalGameReferenceHub hub);
    }

    public class MedalGameStarter : MonoBehaviour
    {
        [SerializeField] GameObject MedalGame_UI;
        MedalGameSceneResult _currentScene;

        public void LoadMedalGameScene(int initialMedalCount = 10)
        {
            var operation = new MedalGameOperation
            {
                InitialMedalCount = initialMedalCount
            };

            _currentScene = MedalGameLauncher.Instance.LaunchAsync(operation);
            _currentScene.OnPrepared += OnScenePrepared;
        }

        void OnScenePrepared(AsyncOperation asyncOperation)
        {
            _currentScene.Activate(OnSceneActivated);
        }

        void OnSceneActivated()
        {
            StartCoroutine(NotifySceneActivated());
            MedalGame_UI.gameObject.SetActive(true);
        }

        IEnumerator NotifySceneActivated()
        {
            while (_currentScene == null || !_currentScene.Scene.IsValid() || !_currentScene.Scene.isLoaded)
            {
                yield return null;
            }

            MedalGameReferenceHub hub = null;

            while (hub == null)
            {
                yield return null;

                var rootObjects = _currentScene.Scene.GetRootGameObjects();
                foreach (var rootObject in rootObjects)
                {
                    hub = rootObject.GetComponent<MedalGameReferenceHub>();
                    if (hub != null) break;
                }
            }

            var receivers = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

            foreach (var receiver in receivers)
            {
                if (receiver is IMedalGameSceneLoadNotifier hubReceiver)
                {
                    hubReceiver.OnMedalGameSceneActivate(hub);
                }
            }
        }

        public void CloseMedalGame()
        {
            if (_currentScene == null || !_currentScene.Scene.IsValid())
            {
                Debug.LogWarning("No active MedalGame scene to close");
                return;
            }

            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(_currentScene.Scene);
            _currentScene = null;
        }
    }
}
