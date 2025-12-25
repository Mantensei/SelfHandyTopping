using System.Collections.Generic;
using System.Linq;
using MantenseiLib;
using MedalGame;
using UnityEngine;

namespace MantenseiNobel.Mouhitotsu
{
    public class CageDistributor : MonoBehaviour, IMedalGameSceneLoadNotifier
    {
        [SerializeField]
        CageOwnershipMarker _defaultMarkerPrefab;

        [SerializeField]
        List<CageOwnershipMarker> _markerPrefabs;

        MedalGameReferenceHub _hub;

        CageManager CageManager => _hub.CageManager;

        readonly Dictionary<CageOwnershipMarker, List<Cage>> _markerCages = new();

        public void OnMedalGameSceneActivate(MedalGameReferenceHub hub)
        {
            _hub = hub;
            var players = PlayerManager.Instance.Players;
            DistributeCages(players.ToArray());
        }

        public void DistributeCages(Player[] players)
        {
            var allCages = CageManager.AllCages.Shuffle().ToList();
            _markerCages.Clear();

            int cageIndex = 0;
            foreach (var player in players.Where(x => x.Alive))
            {
                var markerPrefab = _markerPrefabs.FirstOrDefault(m => m.ID == player.ID);
                if (markerPrefab == null) markerPrefab = _defaultMarkerPrefab;

                if (!_markerCages.ContainsKey(markerPrefab))
                {
                    _markerCages[markerPrefab] = new List<Cage>();
                }

                for (int i = 0; i < player.SkillCount; i++)
                {
                    var cage = allCages[cageIndex++];
                    _markerCages[markerPrefab].Add(cage);

                    if (!cage.TryGetComponent<CageOwnershipMarker>(out _))
                    {
                        var marker = Instantiate(markerPrefab, cage.transform);
                        marker.SetOwner(player);
                    }
                }
            }

            if (!_markerCages.ContainsKey(_defaultMarkerPrefab))
            {
                _markerCages[_defaultMarkerPrefab] = new List<Cage>();
            }

            while (cageIndex < allCages.Count)
            {
                var cage = allCages[cageIndex++];
                _markerCages[_defaultMarkerPrefab].Add(cage);

                if (!cage.TryGetComponent<CageOwnershipMarker>(out _))
                {
                    Instantiate(_defaultMarkerPrefab, cage.transform);
                }
            }
        }

        public IReadOnlyList<Cage> GetMarkerCages(CageOwnershipMarker markerPrefab)
        {
            return _markerCages.TryGetValue(markerPrefab, out var cages) ? cages : new List<Cage>();
        }

        public IEnumerable<CageOwnershipMarker> GetAllMarkers()
        {
            return _markerCages.Keys;
        }
    }
}
