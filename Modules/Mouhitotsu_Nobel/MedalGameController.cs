using MedalGame;
using NUnit.Framework.Constraints;
using UnityEngine;

namespace MantenseiNobel.Mouhitotsu
{
    public class MedalGameController : MonoBehaviour, IMedalGameSceneLoadNotifier
    {
        [SerializeField]
        int _initialMedalCount = 10;

        [SerializeField]
        float _generateDelay = 0.5f;

        MedalGameReferenceHub _hub;

        public void OnMedalGameSceneActivate(MedalGameReferenceHub hub)
        {
            _hub = hub;
            _hub.Loader.OnPoolAllMedal += HandleAllMedalsPooled;
        }

        public void StartMedalGame()
        {
            var config = new MedalGameConfig()
            {
                InitialMedalCount = _initialMedalCount,
            };
            _hub.GameManager.StartGame(config);
        }

        void HandleAllMedalsPooled()
        {
            _hub.Loader.ShootPooledMedals();
        }

        void OnDestroy()
        {
            _hub.Loader.OnPoolAllMedal -= HandleAllMedalsPooled;
        }
    }
}
