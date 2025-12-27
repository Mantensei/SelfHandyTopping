using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MantenseiLib;
using MantenseiNovel;
using MedalGame;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MantenseiNovel.Mouhitotsu
{

    //突貫工事で完成させるための暫定的なんでもあり神クラス
    public class GameEventManager : MonoBehaviour, IMedalGameLoadCompleteReceiver
    {
        Player _mainPlayer => PlayerManager.Instance.MainPlayer;
        NovelScenarioManager scenarioManager => NovelScenarioManager.Instance;

        [SerializeField] TextMeshProUGUI _title_txt;
        string[] _chapterTitles = new[]
        {
            "『普通コンプレックス』",
            "『対戦結果１』",
            "『対戦結果２』",
            "『対戦結果３』",
            "『優勝賞品』",
        };

        void Start()
        {
            GameManager.Instance.OnRegistGameResult += OnRegistGameResult;
            scenarioManager.OnScenarioComplete += OnScenarioComplete;
        }

        void OnScenarioComplete(NovelScenario scenario)
        {
            switch (scenario.ScenarioName)
            {
                case "Chap1_2":
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

            if (bestPlayers.Length == 1 && bestPlayers.Contains(_mainPlayer))
            {
                Win(result);
            }
            else
            {
                Lose(result);
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


            {
                _title_txt.text = _chapterTitles[result.Phase];
            }


            LoadNovelScene(result.Phase);
        }

        void Lose(GameResult result)
        {
            _mainPlayer.Alive = false;

            Destroy(GameManager.Instance.gameObject);
            LoadNovelScene(1);
        }

        void LoadNovelScene(int num)
        {
            SceneManager.LoadScene("Novel_0" + num);
        }

        public void OnMedalGameLoaded(MedalGameReferenceHub hub)
        {
            scenarioManager.gameObject.SetActive(false);
            hub.GameManager.OnGameOver += () =>
            scenarioManager.gameObject.SetActive(true);
        }

        IEnumerable<string> GetWinDialogs()
        {
            yield return "ぎゃー！";
            if (_mainPlayer.UsedSkillCount == 0)
                yield return WinDialog(PersonalityType.None);
            else
                yield return WinDialog(_mainPlayer.UsedleSkills.GetRandomElementOrDefault().PersonalityType);
            yield return "つぎこそ勝つよ！";
        }

        string WinDialog(PersonalityType personality)
        {
            switch (personality)
            {
                case PersonalityType.None:
                    return "わーい♥　勝ったよー！";
                case PersonalityType.Kindness:
                    return "ふん、ザコどもめ...";
                case PersonalityType.Money:
                    return "賞金とかないの？　...ないの...！？";
                case PersonalityType.Intelligence:
                    return "ねーねー、これなにするあそびなのー？";
                case PersonalityType.Health:
                    return "ゲホゲホッ...30分休憩させて...";
                case PersonalityType.Sense:
                    return "ククク...力が抑えきれぬわ...！";
                case PersonalityType.Luck:
                    return "やったー！...ところで、私の財布知らない？";
            }
            return null;
        }

        IEnumerable<(string log, PersonalityType personality)> GetLogs()
        {
            (string, PersonalityType) GetLog(PersonalityType personality)
            {
                var used = _mainPlayer
                .Personalities
                .FirstOrDefault(x => x.PersonalityType == personality)
                .Used;
                return (ProposeDialog(personality, !used), personality);
            }

            yield return GetLog(PersonalityType.Kindness);
            yield return GetLog(PersonalityType.Health);
            yield return GetLog(PersonalityType.Money);
            yield return GetLog(PersonalityType.Intelligence);
            yield return GetLog(PersonalityType.Sense);
            yield return GetLog(PersonalityType.Luck);
        }

        string ProposeDialog(PersonalityType personality, bool value)
        {
            switch (personality)
            {
                case PersonalityType.None:
                    return null;
                case PersonalityType.Kindness:
                    return value ?
                    "あのねっ！　風ちゃん...！"
                    : "おいてめぇ、ツラかせや";
                case PersonalityType.Health:
                    return value ?
                    "えへへ...キンチョーでドキドキしてきた..."
                    : "うっ、動悸が...ゲホゲホッ...コヒュー...";
                case PersonalityType.Money:
                    return value ?
                    "サッカーで一生懸命なところがステキだなあって思ってて..."
                    : "サッカーでいっぱい稼げそうだからステキだなあって思ってて...";
                case PersonalityType.Intelligence:
                    return value ?
                    "いつもカッコよくて、尊敬しています！"
                    : "えっとねー、すきすき！だいすき！";
                case PersonalityType.Sense:
                    return value ?
                    "私と、お友達になってもらえませんか...？"
                    : " 我と永遠（とわ）の契りを結びたまえ！";
                case PersonalityType.Luck:
                    return value ?
                    "ダメ、かな...？"
                    : "断ると、南南西からの凶兆により不幸に見舞われますよ！";

            }
            return null;
        }
    }
}