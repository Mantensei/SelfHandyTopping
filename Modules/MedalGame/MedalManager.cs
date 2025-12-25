using System.Collections.Generic;
using System.Linq;
using MantenseiLib;
using UnityEngine;

namespace MedalGame
{
    [CommonReference]
    public class MedalManager : MonoBehaviour, IMedalGameStartReceiver
    {
        MedalGameReferenceHub Hub => MedalGameReferenceHub.Instance;
        MedalLoader loader => Hub.Loader;

        [SerializeField] Medal _medalPrefab;
        HashSet<Medal> _medals = new();

        public IReadOnlyList<Medal> Medals => _medals.ToList();

        public void OnMedalGameStart(MedalGameConfig config)
        {
            GenerateMedal(config.InitialMedalCount);
        }

        void GenerateMedal(int count = 1, float delay = 0.5f)
        {
            for (int i = 0; i < count; i++)
            {
                loader.GenerateDelayed(_medalPrefab, i * delay, m => RegisterMedal(m));
            }
        }

        public void RegisterMedal(Medal medal)
        {
            _medals.Add(medal);
        }

        public void UnregisterMedal(Medal medal)
        {
            _medals.Remove(medal);
        }
    }
}
