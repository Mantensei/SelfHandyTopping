using System.Collections.Generic;
using MantenseiLib;
using UnityEngine;

namespace MantenseiNovel.Mouhitotsu
{
    public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
    {
        [SerializeField]
        List<Player> _playerPrefabs = new();

        [SerializeField]
        List<Personality> _commonPersonalities = new();

        List<Player> _players;
        public IReadOnlyList<Player> Players => _players;

        public void GeneratePlayers()
        {
            if (_players != null) return;
            _players = new List<Player>();

            foreach (var playerPrefab in _playerPrefabs)
            {
                var player = Instantiate(playerPrefab);

                foreach (var personality in _commonPersonalities)
                {
                    var personalityInstance = Instantiate(personality);
                    player.AddPersonality(personalityInstance);
                }

                _players.Add(player);
            }
        }

        public Player MainPlayer => GetPlayer("Main");
        public Player GetPlayer(string id)
        {
            return _players.Find(p => p.ID == id);
        }
    }
}
