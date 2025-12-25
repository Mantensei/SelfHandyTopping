using System;
using MantenseiLib;
using MedalGame;
using UnityEngine;

namespace MantenseiNobel.Mouhitotsu
{
    public class CageOwnershipMarker : MonoBehaviour, IMedalReceiver
    {
        MedalGameReferenceHub _hub;
        MedalGameReferenceHub Hub
        {
            get
            {
                if (!_hub.IsSafe())
                    _hub = FindAnyObjectByType<MedalGameReferenceHub>();
                return _hub;
            }
        }
        MedalLoader Loader => Hub.Loader;

        [SerializeField]
        string _id;
        public string ID => _id;

        Player _owner;
        public Player Owner => _owner;

        public void SetOwner(Player player)
        {
            _owner = player;
        }
        public event Action<Player, Medal> OnMedalReceived;

        public void OnGetMedal(Medal medal)
        {
            if (Owner == null)
            {
                Debug.LogWarning("Cage has no owner");
                return;
            }

            OnMedalReceived?.Invoke(Owner, medal);
            Loader.Pool(medal);
        }
    }
}
