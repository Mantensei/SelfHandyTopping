using MantenseiLib;
using UnityEngine;

namespace MedalGame
{
    public interface IMedalReceiver
    {
        void OnGetMedal(Medal medal);
    }

    public class MedalDetector : MonoBehaviour
    {
        // [GetComponents(HierarchyRelation.Self | HierarchyRelation.Children)]
        // IMedalReceiver[] _medalReceivers;

        [Parent]
        Cage cage;

        void OnTriggerEnter2D(Collider2D other)
        {
            var parent = cage.Safe()?.transform ?? transform;

            if (other.TryGetComponent<Medal>(out var medal))
            {
                foreach (var receiver in parent.GetComponentsInChildren<IMedalReceiver>())
                {
                    receiver.OnGetMedal(medal);
                }
            }
        }
    }
}
