using System;
using MantenseiLib;
using UnityEngine;

namespace MedalGame
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CommonReferenceAttribute : Attribute
    {

    }

    public class MedalGameReferenceHub : MonoBehaviour
    {
        [GetComponent(HierarchyRelation.Self | HierarchyRelation.Children)]
        public MedalGameManager GameManager { get; private set; }

        [GetComponent(HierarchyRelation.Children)]
        public MedalLoader Loader { get; private set; }

        [GetComponent(HierarchyRelation.Children)]
        public MedalManager MedalManager { get; private set; }

        [GetComponent(HierarchyRelation.Children)]
        public CageManager CageManager { get; private set; }

    }
}
