using MantenseiLib;
using UnityEngine;

namespace MedalGame
{
    public class MedalGameEventMonitorAndExecutor : MonoBehaviour
    {
        [GetComponent(HierarchyRelation.Parent)]
        MedalGameReferenceHub _hub;

        [SerializeField] int _initialMedalCount = 10;
        [SerializeField] float _generateDelay = 0.5f;

        // void Start()
        // {
        //     _hub ??= Object.FindAnyObjectByType<MedalGameReferenceHub>();

        //     _hub.Loader.OnPoolAllMedal += HandleAllMedalsPooled;
        //     _hub.MedalManager.GenerateMedal(_initialMedalCount, _generateDelay);
        // }

        // void HandleAllMedalsPooled()
        // {
        //     _hub.Loader.ShootPooledMedals();
        // }
    }
}
