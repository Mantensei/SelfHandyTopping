using System.Linq;
using MantenseiLib;
using UnityEngine;

#if UNITY_EDITOR
namespace MantenseiNobel.Mouhitotsu
{
    public class ScoreDebugDisplay : MonoBehaviour
    {
        [GetComponent] ScoreManager _scoreManager;
        PlayerManager _playerManager => PlayerManager.Instance;

        [SerializeField] Vector2 _position = new Vector2(10, 10);
        [SerializeField] int _fontSize = 24;
        [SerializeField] Color _textColor = Color.white;

        void OnGUI()
        {
            if (_scoreManager == null || _playerManager == null) return;

            float yOffset = _position.y;

            var headerStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = _fontSize,
                normal = { textColor = _textColor }
            };
            GUI.Label(new Rect(_position.x, yOffset, 500, 30), "=== SCORES ===", headerStyle);
            yOffset += 30;

            foreach (var kvp in _scoreManager.AllScores)
            {
                var player = _playerManager.GetPlayer(kvp.Key);
                var playerColor = player != null ? player.Color : _textColor;

                var style = new GUIStyle(GUI.skin.label)
                {
                    fontSize = _fontSize,
                    normal = { textColor = playerColor }
                };

                var paddedId = kvp.Key.FirstOrDefault();
                var circles = new string('●', Mathf.Max(0, kvp.Value));
                var displayText = $"{paddedId} {circles} [ {kvp.Value} ]";

                GUI.Label(new Rect(_position.x, yOffset, 500, 30), displayText, style);
                yOffset += 30;
            }
        }
    }
}
#endif