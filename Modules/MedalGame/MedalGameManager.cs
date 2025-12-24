using MantenseiLib;
using UnityEngine;

namespace MedalGame
{
    public enum GameStatus
    {
        Idle,
        Playing,
        GameOver
    }

    public interface IMedalGameStartReceiver
    {
        void OnMedalGameStart(MedalGameConfig config);
    }

    public class MedalGameConfig
    {
        public int InitialMedalCount { get; set; } = 10;
    }

    [CommonReference]
    public class MedalGameManager : MonoBehaviour
    {
        [GetComponent(HierarchyRelation.Self | HierarchyRelation.Parent)]
        MedalGameReferenceHub _hub;

        GameStatus _status = GameStatus.Idle;

        public MedalGameReferenceHub Hub => _hub;
        public GameStatus Status => _status;

        public void StartGame(MedalGameConfig config)
        {
            if (_status == GameStatus.Playing)
            {
                Debug.LogWarning("Game is already playing");
                return;
            }

            _status = GameStatus.Playing;

            var receivers = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            foreach (var receiver in receivers)
            {
                if (receiver is IMedalGameStartReceiver startReceiver)
                {
                    startReceiver.OnMedalGameStart(config);
                }
            }
        }

        public void GameOver()
        {
            if (_status != GameStatus.Playing)
            {
                Debug.LogWarning("Game is not playing");
                return;
            }

            _status = GameStatus.GameOver;
        }
    }
}
