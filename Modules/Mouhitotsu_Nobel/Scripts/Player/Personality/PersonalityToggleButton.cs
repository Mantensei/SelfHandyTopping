using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MantenseiNobel.Mouhitotsu
{
    public class PersonalityToggleButton : MonoBehaviour
    {
        [SerializeField] Personality _personality;
        [SerializeField] Toggle _toggle;
        [SerializeField] TextMeshProUGUI _nameText;
        [SerializeField] TextMeshProUGUI _skillNameText;
        [SerializeField] TextMeshProUGUI _descriptionText;

        public Personality Personality => _personality;
        public bool IsSelected => _toggle.isOn;

        void Start()
        {
            if (_personality != null)
            {
                UpdateDisplay();
            }

            _toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        public void SetPersonality(Personality personality)
        {
            _personality = personality;
            UpdateDisplay();
        }

        public void SetInteractable(bool interactable)
        {
            _toggle.interactable = interactable;
        }

        void UpdateDisplay()
        {
            if (_nameText != null)
            {
                _nameText.text = _personality.Name;
            }

            if (_skillNameText != null)
            {
                _skillNameText.text = _personality.SkillName;
            }

            if (_descriptionText != null)
            {
                _descriptionText.text = _personality.SkillDescription;
            }
        }

        void OnToggleValueChanged(bool isOn)
        {
        }

        void OnDestroy()
        {
            _toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
        }
    }
}
