using MantenseiLib;
using MedalGame;
using UnityEngine;

namespace MantenseiNovel.Mouhitotsu
{
    public class NobelReferenceHub : SingletonMonoBehaviour<NobelReferenceHub>
    {
        [GetComponent(HierarchyRelation.Children)]
        public MedalGameController MedalGameController { get; private set; }

        [GetComponent(HierarchyRelation.Children)]
        public MedalGameStarter MedalGameStarter { get; private set; }

        [GetComponent(HierarchyRelation.Children)]
        public CageDistributor CageDistributor { get; private set; }

        [GetComponent(HierarchyRelation.Children)]
        public TableBangController TableBangController { get; private set; }


        // [GetComponent(HierarchyRelation.Children)]
        // public PlayerManager PlayerManager { get; private set; }
        // public PlayerManager PlayerManager { get; private set; }
    }
}
