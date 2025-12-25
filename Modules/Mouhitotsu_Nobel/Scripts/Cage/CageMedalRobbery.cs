using System.Linq;
using MantenseiLib;
using MedalGame;
using UnityEngine;

namespace MantenseiNobel.Mouhitotsu
{
    public class CageMedalRobbery : MonoBehaviour, IMedalReceiver
    {
        [Parent]
        Cage cage;

        [Sibling]
        CageOwnershipMarker _marker;
        string _playerId => _marker.ID;

        ScoreManager _scoreManager => ScoreManager.Instance;
        PlayerManager _playerManager => PlayerManager.Instance;

        int _robberyPower;

        public void Setup(int robberyPower)
        {
            _robberyPower = robberyPower;
        }

        public void OnGetMedal(Medal medal)
        {
            var target = _playerManager.Players
                .Where(x => x.ID != _playerId)
                .Where(x => _scoreManager.GetScore(x.ID) > 0)
                .GetRandomElementOrDefault();

            if (target == null) return;

            int stolenPoints = Mathf.Min(_robberyPower, _scoreManager.GetScore(target.ID));
            if (stolenPoints > 0)
            {
                _scoreManager.RemoveScore(target.ID, stolenPoints);
                _scoreManager.AddScore(_playerId, stolenPoints);
            }
        }
    }
}
