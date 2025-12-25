using System;
using MantenseiLib;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace MantenseiNobel.Mouhitotsu
{
    public class PersonalityToggleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        Personality _personality;
        [GetComponent(HierarchyRelation.Self | HierarchyRelation.Children)]
        Toggle _toggle;
        [SerializeField]
        TextMeshProUGUI _nameText;

        public Personality Personality => _personality;
        public bool IsSelected => _toggle.isOn;

        public event Action<Personality> OnPointerEnterEvent;
        public event Action OnPointerExitEvent;

        public void Setup(Personality personality)
        {
            _personality = personality;
            UpdateDisplay();
            _toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnPointerEnterEvent?.Invoke(_personality);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnPointerExitEvent?.Invoke();
        }

        public void SetInteractable(bool interactable)
        {
            _toggle.interactable = interactable;
        }

        void UpdateDisplay()
        {
            _nameText.text = _personality.Name;
            if (_personality.Used)
            {
                _toggle.interactable = false;
                _nameText.fontStyle = FontStyles.Strikethrough;
            }
            _toggle.isOn = false;
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
