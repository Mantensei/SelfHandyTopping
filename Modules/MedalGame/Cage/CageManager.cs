using System.Collections.Generic;
using MantenseiLib;
using UnityEngine;

namespace MedalGame
{
    [CommonReference]
    public class CageManager : MonoBehaviour
    {
        [GetComponents(HierarchyRelation.Children)]
        Cage[] _cages;

        public IReadOnlyList<Cage> AllCages => _cages;
    }
}
