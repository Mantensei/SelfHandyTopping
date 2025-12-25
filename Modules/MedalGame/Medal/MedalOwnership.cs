using MantenseiNobel.Mouhitotsu;
using UnityEngine;
using UnityEngine.UI;

namespace MedalGame
{
    public class MedalOwnership : MonoBehaviour
    {
        string _ownerPlayerId;

        public string OwnerPlayerId => _ownerPlayerId;
        public bool HasOwner => !string.IsNullOrEmpty(_ownerPlayerId);

        public void SetOwner(string playerId)
        {
            _ownerPlayerId = playerId;
        }

        public bool IsOwnedBy(string playerId)
        {
            if (!HasOwner) return false;
            return _ownerPlayerId == playerId;
        }

        public static bool IsOwnedBy(Medal medal, string playerId)
        {
            if (!medal.TryGetComponent<MedalOwnership>(out var ownership)) return false;
            return ownership.IsOwnedBy(playerId);
        }

        public static MedalOwnership Attach(Medal medal, string playerId)
        {
            var ownership = medal.gameObject.AddComponent<MedalOwnership>();
            ownership.SetOwner(playerId);

            foreach (var sr in medal.GetComponentsInChildren<SpriteRenderer>())
                sr.color = PlayerManager.Instance.GetPlayer(playerId).Color;

            return ownership;
        }
    }
}
