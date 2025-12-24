using MantenseiLib;
using UnityEngine;

namespace MedalGame
{
    public class MedalReloader : MonoBehaviour, IMedalReceiver
    {
        [GetComponent(HierarchyRelation.Parent)]
        MedalGameReferenceHub hub;
        MedalLoader Loader => hub.Loader;

        public void OnGetMedal(Medal medal)
        {
            Loader.Pool(medal);
        }
    }
}
