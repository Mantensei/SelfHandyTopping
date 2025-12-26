using MantenseiLib;
using UnityEngine;

namespace MedalGame
{
    public class MedalReloader : MonoBehaviour, IMedalReceiver
    {
        MedalLoader Loader => MedalGameReferenceHub.Instance.Loader;
        public void OnGetMedal(Medal medal)
        {
            // Loader.Pool(medal);
            medal.gameObject.SetActive(false);
            Loader.ShootDelayed(medal, 0.5f);
        }
    }
}
