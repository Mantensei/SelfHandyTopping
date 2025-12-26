using System;
using System.Collections.Generic;
using System.Linq;
using MantenseiLib;
using MedalGame;
using Unity.VisualScripting;
using UnityEngine;

namespace MantenseiNovel.Mouhitotsu
{
    public class PlayerResult
    {
        public string PlayerId;
        public Player Player => PlayerManager.Instance.GetPlayer(PlayerId);
        public int Rank;
        public int Score;
        public int SessionScore;
    }

    public class GameResult
    {
        public List<PlayerResult> PlayerResults;
        public int TotalScore => PlayerResults?.Sum(r => r.Score) ?? 0;
        public int SessionScore { get; set; }
        public int CarryOver { get; set; }
        public int Phase { get; set; }
    }

    public class GameManager : SingletonMonoBehaviour<GameManager>, IMedalGameLoadCompleteReceiver
    {
        readonly Stack<GameResult> _gameHistory = new();

        public GameResult LastGame => _gameHistory.TryPeek(out var result) ? result : null;

        public event Action<GameResult> OnRegistGameResult;

        void OnGameOver()
        {
            var scoreManager = ScoreManager.Instance;
            var playerManager = PlayerManager.Instance;

            var playerResults = new List<PlayerResult>();

            if (!_gameHistory.TryPeek(out var last))
            {
                last = new GameResult { PlayerResults = new List<PlayerResult>() };
            }

            foreach (var player in playerManager.Players)
            {
                var currentScore = scoreManager.GetScore(player.ID);
                var lastPlayerResult = last.PlayerResults?.FirstOrDefault(r => r.PlayerId == player.ID);
                var lastScore = lastPlayerResult?.Score ?? 0;

                playerResults.Add(new PlayerResult
                {
                    PlayerId = player.ID,
                    Score = currentScore,
                    SessionScore = currentScore - lastScore,
                    Rank = 0
                });
            }

            playerResults = playerResults.OrderByDescending(r => r.Score).ToList();
            for (int i = 0; i < playerResults.Count; i++)
            {
                playerResults[i].Rank = i + 1;
            }

            var currentResult = new GameResult
            {
                PlayerResults = playerResults
            };

            currentResult.SessionScore = currentResult.TotalScore - last.TotalScore;
            currentResult.Phase = last.Phase + 1;

            OnRegistGameResult?.Invoke(currentResult);

            _gameHistory.Push(currentResult);
        }

        public PlayerResult GetLastPlayerResult(string playerId)
        {
            return LastGame?.PlayerResults?.FirstOrDefault(r => r.PlayerId == playerId);
        }

        public void OnMedalGameLoaded(MedalGameReferenceHub hub)
        {
            hub.GameManager.OnGameOver += OnGameOver;
        }

    }
}
