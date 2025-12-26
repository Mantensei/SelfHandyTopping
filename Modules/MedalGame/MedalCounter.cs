using UnityEngine;

namespace MedalGame
{
    public class MedalCounter : MonoBehaviour, IMedalReceiver
    {
        int _medalCount;

        public int MedalCount => _medalCount;

        public void OnGetMedal(Medal medal)
        {
            _medalCount++;
        }
    }
}
