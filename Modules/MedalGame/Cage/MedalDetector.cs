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
        [GetComponents(HierarchyRelation.Children)]
        IMedalReceiver[] _medalReceivers;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Medal>(out var medal))
            {
                foreach (var receiver in _medalReceivers.Safe())
                {
                    receiver.OnGetMedal(medal);
                }
            }
        }
    }
}
