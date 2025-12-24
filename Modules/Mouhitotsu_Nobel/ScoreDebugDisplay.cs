using MantenseiLib;
using UnityEngine;

namespace MantenseiNobel.Mouhitotsu
{
    public class ScoreDebugDisplay : MonoBehaviour
    {
        [GetComponent] ScoreManager _scoreManager;

        [SerializeField] Vector2 _position = new Vector2(10, 10);
        [SerializeField] int _fontSize = 24;
        [SerializeField] Color _textColor = Color.white;

        GUIStyle _style;

        void OnGUI()
        {
            if (_scoreManager == null) return;

            if (_style == null)
            {
                _style = new GUIStyle(GUI.skin.label)
                {
                    fontSize = _fontSize,
                    normal = { textColor = _textColor }
                };
            }

            float yOffset = _position.y;
            GUI.Label(new Rect(_position.x, yOffset, 300, 30), "=== SCORES ===", _style);
            yOffset += 30;

            foreach (var kvp in _scoreManager.AllScores)
            {
                GUI.Label(new Rect(_position.x, yOffset, 300, 30), $"{kvp.Key}: {kvp.Value}", _style);
                yOffset += 30;
            }
        }
    }
}
