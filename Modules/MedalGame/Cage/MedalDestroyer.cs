using UnityEngine;

namespace MedalGame
{
    public class MedalDestroyer : MonoBehaviour, IMedalReceiver
    {
        public void OnGetMedal(Medal medal)
        {
            Destroy(medal.gameObject);
        }
    }
}
