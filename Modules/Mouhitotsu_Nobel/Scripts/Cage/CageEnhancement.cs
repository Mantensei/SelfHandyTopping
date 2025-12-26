using System.Linq;
using MantenseiLib;
using MedalGame;
using Unity.VisualScripting;
using UnityEngine;

namespace MantenseiNovel.Mouhitotsu
{
    public class CageEnhancement : MonoBehaviour
    {
        [SerializeField]
        int _level;
        public int Level => _level;

        [SerializeField]
        int _bonus = 0;
        public int Bonus => _bonus;

        [Parent]
        Cage cage;

        [Sibling]
        CageScoreAdder _scoreAdder;

        void Start()
        {
            _scoreAdder.ScoreMultiplier += Bonus;
        }

        public void LevelUp(int add)
        {
            var targetEnhancement = GetCageEnhancement(Level + add);
            ReplaceEnhancement(gameObject, targetEnhancement.gameObject);
        }

        public static void LevelUp(Cage cage, int add)
        {
            var existing = cage.GetComponentInChildren<CageEnhancement>();
            if (existing != null)
            {
                existing.LevelUp(add);
                return;
            }

            var targetEnhancement = GetCageEnhancement(add);
            Instantiate(targetEnhancement.gameObject, cage.transform);
        }

        static void ReplaceEnhancement(GameObject oldEnhancement, GameObject newEnhancementPrefab)
        {
            var newEnhancement = Instantiate(newEnhancementPrefab, oldEnhancement.transform.parent);
            newEnhancement.transform.SetSiblingIndex(oldEnhancement.transform.GetSiblingIndex());
            Destroy(oldEnhancement);
        }

        static CageEnhancement GetCageEnhancement(int targetLevel)
        {
            var enhancements = ResourceManager.GetResources<CageEnhancement>();

            var exact = enhancements.FirstOrDefault(e => e.Level == targetLevel);
            if (exact != null) return exact;

            return enhancements
                .Where(e => e.Level < targetLevel)
                .OrderByDescending(e => e.Level)
                .FirstOrDefault();
        }
    }
}
