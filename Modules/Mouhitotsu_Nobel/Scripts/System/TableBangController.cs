using System.Collections;
using MantenseiLib;
using MedalGame;
using UnityEngine;

namespace MantenseiNovel.Mouhitotsu
{
    public class TableBangController : MonoBehaviour, IMedalGameSceneLoadNotifier
    {
        [SerializeField]
        float _bangForce = 10f;

        [SerializeField]
        float _cooldownTime = 2f;

        [SerializeField]
        float _shakeIntensity = 0.5f;

        [SerializeField]
        float _shakeDuration = 0.3f;

        MedalGameReferenceHub _hub;
        MedalManager MedalManager => _hub.MedalManager;

        Camera _camera;
        Vector3 _originalCameraPosition;

        float _lastBangTime = -999f;

        bool CanBang => Time.time >= _lastBangTime + _cooldownTime;

        public void OnMedalGameSceneActivate(MedalGameReferenceHub hub)
        {
            _hub = hub;
        }

        void Start()
        {
            _camera = Camera.main;
            _originalCameraPosition = _camera.transform.localPosition;
        }

        void Update()
        {
            if (Input.anyKeyDown && CanBang)
            {
                Bang();
            }
        }

        void Bang()
        {
            if (_hub == null || _hub.GameManager.Status != GameStatus.Playing) return;

            _lastBangTime = Time.time;

            var medals = MedalManager.Medals;

            var randomDirection = new Vector2(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            ).normalized * _bangForce;

            foreach (var medal in medals)
            {
                if (medal.TryGetComponent<IMoverProvider>(out var mover))
                {
                    mover.Reference.Move(new MoveCommand(randomDirection));
                }
            }

            if (_camera != null)
            {
                StartCoroutine(ShakeCamera());
            }
        }

        IEnumerator ShakeCamera()
        {
            float elapsed = 0f;

            while (elapsed < _shakeDuration)
            {
                elapsed += Time.deltaTime;

                float progress = elapsed / _shakeDuration;
                float dampening = 1f - progress;

                Vector3 offset = new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(0f, 1f),
                    0f
                ) * _shakeIntensity * dampening;

                _camera.transform.localPosition = _originalCameraPosition + offset;

                yield return null;
            }

            _camera.transform.localPosition = _originalCameraPosition;
        }
    }
}
