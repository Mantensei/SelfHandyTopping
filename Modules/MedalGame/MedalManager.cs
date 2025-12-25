using System;
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
        [SerializeField] float _generateDelay = 0.1f;
        HashSet<Medal> _medals = new();

        int _queuedGenerateCount = 0;

        public IReadOnlyList<Medal> Medals => _medals.ToList();

        public void OnMedalGameStart(MedalGameConfig config)
        {
            GenerateMedal(config.InitialMedalCount);
        }

        public void GenerateMedal(Action<Medal> onGenerated) => GenerateMedal(1, onGenerated);
        public void GenerateMedal(int count = 1, Action<Medal> onGenerated = null)
        {
            for (int i = 0; i < count; i++)
            {
                int currentIndex = _queuedGenerateCount;
                _queuedGenerateCount++;

                loader.GenerateDelayed(_medalPrefab, currentIndex * _generateDelay, m =>
                {
                    RegisterMedal(m);
                    onGenerated?.Invoke(m);
                });
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
