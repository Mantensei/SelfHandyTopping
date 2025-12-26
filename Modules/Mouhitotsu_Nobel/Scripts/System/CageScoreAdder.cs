using MantenseiLib;
using MedalGame;
using UnityEngine;

namespace MantenseiNovel.Mouhitotsu
{
    public class CageScoreAdder : MonoBehaviour, IMedalReceiver
    {
        [Parent]
        CageOwnershipMarker marker;
        string _playerId => marker.ID;

        ScoreManager _scoreManager => ScoreManager.Instance;

        public int ScoreMultiplier { get; set; } = 1;

        public void OnGetMedal(Medal medal)
        {
            if (!medal.TryGetComponent<MedalOwnership>(out var ownership))
            {
                _scoreManager.AddScore(_playerId, ScoreMultiplier);
                return;
            }

            if (ownership.IsOwnedBy(_playerId))
            {
                _scoreManager.AddScore(_playerId, ScoreMultiplier);
            }
        }
    }
}
