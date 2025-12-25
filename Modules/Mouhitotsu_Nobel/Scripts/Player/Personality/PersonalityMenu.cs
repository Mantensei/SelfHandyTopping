using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MantenseiLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MantenseiNobel.Mouhitotsu
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
                .ToList();

            foreach (var personality in selectedPersonalities)
            {
                var context = new SkillExecuteContext(_medalGameController.Hub, _currentPlayer);
                personality.Execute(context);
            }

            _medalGameController.StartMedalGame();
            gameObject.SetActive(false);
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
