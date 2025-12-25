using MedalGame;
using UnityEngine;

namespace MantenseiNobel.Mouhitotsu
{
    public class CageBlockade : MonoBehaviour
    {
        [SerializeField] int _hitPoints = 1;

        int _currentHP;

        void Awake()
        {
            _currentHP = _hitPoints;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.TryGetComponent<Medal>(out _)) return;

            _currentHP--;
            if (_currentHP <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
