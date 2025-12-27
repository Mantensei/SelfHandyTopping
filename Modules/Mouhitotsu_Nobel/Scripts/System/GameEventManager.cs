using System.Linq;
using MantenseiLib;
using MantenseiNovel;
using MedalGame;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MantenseiNovel.Mouhitotsu
{

    //突貫工事で完成させるための暫定的なんでもあり神クラス
    public class GameEventManager : MonoBehaviour
    {
        Player main => PlayerManager.Instance.MainPlayer;

        void Start()
        {
            GameManager.Instance.OnRegistGameResult += OnRegistGameResult;
            Debug.Log(NovelScenarioManager.Instance);
            NovelScenarioManager.Instance.OnScenarioComplete += OnScenarioComplete;
        }

        void OnScenarioComplete(NovelScenario scenario)
        {
            switch (scenario.ScenarioName)
            {
                case "OP_2":
                    NobelReferenceHub.Instance.MedalGameStarter.LoadMedalGameScene();
                    break;
                default:
                    break;
            }
        }

        void OnRegistGameResult(GameResult result)
        {
            var destroyArea = GameObject.Find("DestroyArea");
            if (destroyArea == null) return;

            if (!destroyArea.TryGetComponent<MedalCounter>(out var medalCounter)) return;

            result.CarryOver = medalCounter.MedalCount;

            var maxScore = result
                .PlayerResults
                .Where(x => x.Player.Alive)
                .Max(x => x.Score);

            var bestPlayers = result
                .PlayerResults
                .Where(x => x.Player.Alive && x.Score == maxScore)
                .Select(x => x.Player)
                .ToArray();

            if (bestPlayers.Length != 1 || !bestPlayers.Contains(main))
            {
                Lose(result);
            }
            else
            {
                Win(result);
            }
        }

        void Win(GameResult result)
        {
            var worst = result
                .PlayerResults
                .Where(x => x.Player.Alive)
                .OrderBy(x => x.Score)
                .FirstOrDefault()
                .Player;

            worst.Alive = false;

            SceneManager.LoadScene("Nobel_01");
        }

        void Lose(GameResult result)
        {
            main.Alive = false;

            Destroy(GameManager.Instance.gameObject);
            SceneManager.LoadScene("Nobel_01");
        }

        void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnRegistGameResult -= OnRegistGameResult;
            }

            if (NovelScenarioManager.Instance != null)
            {
                NovelScenarioManager.Instance.OnScenarioComplete -= OnScenarioComplete;
            }
        }
    }
}
