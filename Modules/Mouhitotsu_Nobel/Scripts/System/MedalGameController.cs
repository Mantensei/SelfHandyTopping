using System.Linq;
using MantenseiLib;
using MedalGame;
using NUnit.Framework.Constraints;
using UnityEngine;

namespace MantenseiNovel.Mouhitotsu
{
    public class MedalGameController : MonoBehaviour, IMedalGameSceneLoadNotifier
    {
        [SerializeField]
        int _initialMedalCount = 10;

        [SerializeField]
        float _generateDelay = 0.5f;

        MedalGameReferenceHub _hub;

        public MedalGameReferenceHub Hub => _hub;

        public void OnMedalGameSceneActivate(MedalGameReferenceHub hub)
        {
            _hub = hub;
            _hub.Loader.OnPoolAllMedal += HandleAllMedalsPooled;
        }

        public void StartMedalGame()
        {
            var config = new MedalGameConfig()
            {
                InitialMedalCount = 0,
            };
            _hub.GameManager.StartGame(config);

            GenerateInitialMedals();
        }

        void GenerateInitialMedals()
        {
            _hub.MedalManager.GenerateMedal(_initialMedalCount);

            var lastGame = GameManager.Instance.LastGame;
            if (lastGame == null) return;

            var carryOver = lastGame.CarryOver;
            _hub.MedalManager.GenerateMedal(carryOver);

            // foreach (var playerResult in lastGame.PlayerResults)
            // {
            //     var player = playerResult.Player;
            //     if (!player.Alive) continue;

            //     int medalCount = playerResult.SessionScore;

            //     _hub.MedalManager.GenerateMedal(medalCount, m => MedalOwnership.Attach(m, player.ID));
            // }
        }

        void GenerateInitialMedalsOld()
        {
            var lastGame = GameManager.Instance.LastGame;
            var carryOver = lastGame?.CarryOver ?? 0;

            for (int i = 0; i < _initialMedalCount; i++)
            {
                _hub.MedalManager.GenerateMedal();
            }

            if (carryOver == 0) return;

            var playerManager = PlayerManager.Instance;
            var scoreManager = ScoreManager.Instance;

            int neutralMedals = carryOver / 2;
            for (int i = 0; i < neutralMedals; i++)
            {
                _hub.MedalManager.GenerateMedal();
            }

            var rankedPlayers = playerManager.Players
                .Select(p => new { Player = p, Score = scoreManager.GetScore(p.ID) })
                .OrderByDescending(x => x.Score)
                .Take(3)
                .ToList();

            int remainingMedals = carryOver - neutralMedals;
            int[] rankDistribution = { remainingMedals / 2, remainingMedals / 4, remainingMedals / 8 };

            for (int i = 0; i < rankedPlayers.Count; i++)
            {
                var playerData = rankedPlayers[i];
                int medalCount = rankDistribution[i];

                for (int j = 0; j < medalCount; j++)
                {
                    _hub.MedalManager.GenerateMedal(m => MedalOwnership.Attach(m, playerData.Player.ID));
                }
            }
        }

        void HandleAllMedalsPooled()
        {
            _hub.Loader.ShootPooledMedals();
        }

        void OnDestroy()
        {
            if (_hub.IsSafe())
                _hub.Loader.OnPoolAllMedal -= HandleAllMedalsPooled;
        }
    }
}
