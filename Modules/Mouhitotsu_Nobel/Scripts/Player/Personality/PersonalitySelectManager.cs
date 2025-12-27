// using System.Collections.Generic;
// using System.Linq;
// using MedalGame;
// using UnityEngine;
// using UnityEngine.UI;

// namespace MantenseiNobel.Mouhitotsu
// {
//     public class PersonalitySelector : MonoBehaviour
//     {
//         [SerializeField] List<PersonalityToggleButton> _toggleButtons = new();
//         [SerializeField] Button _confirmButton;
//         [SerializeField] MedalGameController _medalGameController;

//         Player _currentPlayer;

//         void Start()
//         {
//             _confirmButton.onClick.AddListener(OnConfirmButtonClicked);
//         }

//         public void SetupForPlayer(Player player)
//         {
//             _currentPlayer = player;

//             foreach (var toggleButton in _toggleButtons)
//             {
//                 toggleButton.SetInteractable(true);
//             }
//         }

//         void OnConfirmButtonClicked()
//         {
//             var selectedPersonalities = _toggleButtons
//                 .Where(button => button.IsSelected)
//                 .Select(button => button.Personality)
//                 .ToList();

//             if (selectedPersonalities.Count == 0)
//             {
//                 Debug.LogWarning("No personalities selected");
//                 return;
//             }

//             _medalGameController.StartMedalGame();

//             foreach (var toggleButton in _toggleButtons)
//             {
//                 toggleButton.SetInteractable(false);
//             }
//         }

//         void OnDestroy()
//         {
//             _confirmButton.onClick.RemoveListener(OnConfirmButtonClicked);
//         }
//     }
// }
