using MantenseiLib;
using UnityEngine;

namespace MedalGame
{
    public class MedalDestroyer : MonoBehaviour, IMedalReceiver
    {
        [GetComponent(HierarchyRelation.Parent)]
        MedalGameReferenceHub Hub;

        MedalLoader Loader => Hub.Loader;

        public void OnGetMedal(Medal medal)
        {
            Loader.DestroyMedal(medal);
        }
    }
}
