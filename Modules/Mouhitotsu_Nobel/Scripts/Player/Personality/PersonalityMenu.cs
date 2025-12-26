using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MantenseiLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MantenseiNovel.Mouhitotsu
{
    public class PersonalityMenu : MonoBehaviour
    {
        [SerializeField]
        PersonalityToggleButton _toggleButtonPrefab;

        [SerializeField]
        Transform _toggleButtonParent;

        [SerializeField]
        TextMeshProUGUI _descriptionText;

        [GetComponent(HierarchyRelation.Children)]
        Button _confirmButton;

        MedalGameController _medalGameController => NobelReferenceHub.Instance.MedalGameController;

        readonly List<PersonalityToggleButton> _toggleButtons = new();

        Player _currentPlayer;
        PlayerManager playerManager => PlayerManager.Instance;

        void Start()
        {
            _confirmButton.onClick.AddListener(OnConfirmButtonClicked);
            Setup(playerManager.MainPlayer);
        }

        public void Setup(Player player)
        {
            _currentPlayer = player;
            GenerateToggleButtons();
        }

        void GenerateToggleButtons()
        {
            foreach (var personality in _currentPlayer.Personalities.OrderBy(x => x.PersonalityType))
            {
                var toggleButton = Instantiate(_toggleButtonPrefab, _toggleButtonParent);
                toggleButton.Setup(personality);
                toggleButton.OnPointerEnterEvent += OnPersonalityHoverEnter;
                toggleButton.OnPointerExitEvent += OnPersonalityHoverExit;
                _toggleButtons.Add(toggleButton);
            }
        }

        void OnPersonalityHoverEnter(Personality personality)
        {
            _descriptionText.text = personality.SkillDescription;
        }

        void OnPersonalityHoverExit()
        {
            _descriptionText.text = "";
        }

        void OnConfirmButtonClicked()
        {
            var selectedPersonalities = _toggleButtons
                .Where(x => x.IsSelected)
                .Select(x => x.Personality)
                .OrderByDescending(x => x.ExecutionPriority)
                .ToArray();

            var context = new SkillExecuteContext(_medalGameController.Hub, _currentPlayer, selectedPersonalities);

            foreach (var personality in selectedPersonalities)
            {
                personality.Execute(context);
            }

            // ExecuteAISkills();

            _medalGameController.StartMedalGame();
            gameObject.SetActive(false);
        }

        void ExecuteAISkills()
        {
            var playerManager = PlayerManager.Instance;
            var aiPlayers = playerManager.Players.Where(p => p != playerManager.MainPlayer);

            foreach (var aiPlayer in aiPlayers)
            {
                var allSkills = aiPlayer.Personalities.ToList();
                var skillCount = Random.Range(0, Mathf.Min(2, allSkills.Count + 1));
                var selectedSkills = allSkills.Shuffle().Take(skillCount).OrderByDescending(x => x.ExecutionPriority).ToArray();

                var context = new SkillExecuteContext(_medalGameController.Hub, aiPlayer, selectedSkills);
                foreach (var skill in selectedSkills)
                {
                    skill.Execute(context);
                }
            }
        }

        void OnDestroy()
        {
            _confirmButton.onClick.RemoveListener(OnConfirmButtonClicked);

            foreach (var toggleButton in _toggleButtons)
            {
                if (toggleButton != null)
                {
                    toggleButton.OnPointerEnterEvent -= OnPersonalityHoverEnter;
                    toggleButton.OnPointerExitEvent -= OnPersonalityHoverExit;
                }
            }
        }
    }
}
