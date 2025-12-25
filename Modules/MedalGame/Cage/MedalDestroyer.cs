using MantenseiLib;
using UnityEngine;

namespace MedalGame
{
    public class MedalDestroyer : MonoBehaviour, IMedalReceiver
    {
        MedalLoader Loader => MedalGameReferenceHub.Instance.Loader;

        public void OnGetMedal(Medal medal)
        {
            Loader.DestroyMedal(medal);
        }
    }
}
