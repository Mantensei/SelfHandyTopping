using System;
using System.Collections.Generic;
using UnityEngine;

namespace MantenseiNobel.Mouhitotsu
{
    public class ScoreManager : MonoBehaviour
    {
        readonly Dictionary<string, int> _scores = new();

        public event Action<string, int> OnScoreChanged;

        public void AddScore(string playerId, int points = 1)
        {
            if (!_scores.ContainsKey(playerId))
            {
                _scores[playerId] = 0;
            }

            _scores[playerId] += points;
            OnScoreChanged?.Invoke(playerId, _scores[playerId]);
        }

        public int GetScore(string playerId)
        {
            return _scores.TryGetValue(playerId, out var score) ? score : 0;
        }

        public IReadOnlyDictionary<string, int> AllScores => _scores;

        public void ResetScore(string playerId)
        {
            if (_scores.ContainsKey(playerId))
            {
                _scores[playerId] = 0;
                OnScoreChanged?.Invoke(playerId, 0);
            }
        }

        public void ResetAllScores()
        {
            foreach (var playerId in _scores.Keys)
            {
                _scores[playerId] = 0;
                OnScoreChanged?.Invoke(playerId, 0);
            }
        }
    }
}
