using System.Linq;
using MantenseiLib;
using MedalGame;
using UnityEngine;

namespace MantenseiNovel.Mouhitotsu
{
    public enum PersonalityType
    {
        None = 0,
        Kindness = 1,
        Money = 2,
        Intelligence = 3,
        Health = 4,
        Sense = 5,
        Luck = 6
    }

    public class SkillExecuteContext
    {
        public MedalGameReferenceHub Hub { get; }
        public Player Owner { get; }
        public Personality[] SelectedSkills { get; }

        public SkillExecuteContext(MedalGameReferenceHub hub, Player owner, Personality[] selectedSkills)
        {
            Hub = hub;
            Owner = owner;
            SelectedSkills = selectedSkills;
        }
    }

    [CreateAssetMenu(fileName = "NewPersonality", menuName = "Mouhitotsu/Personality")]
    public class Personality : ScriptableObject
    {
        [SerializeField] PersonalityType _personalityType = PersonalityType.None;
        [SerializeField] string _name;
        [SerializeField] string _skillName;
        [TextArea]
        [SerializeField] string _skillDescription;
        [SerializeField] float _skillPower = 1f;
        [SerializeField] int _executionPriority = 0;
        public PersonalityType PersonalityType => _personalityType;
        public string Name => _name;
        public string SkillName => _skillName;
        public string SkillDescription => _skillDescription;
        public int SkillLevel { get; set; } = 1;
        public float SkillPower => _skillPower * SkillLevel;
        public int ExecutionPriority => _executionPriority;
        public bool Used { get; private set; } = false;

        public void Execute(SkillExecuteContext context)
        {
            if (Used) return;
            Used = true;

            switch (_personalityType)
            {
                case PersonalityType.Kindness:
                    ExecuteMedalRobbery(context);
                    break;
                case PersonalityType.Money:
                    ExecuteMedalEnhancement(context);
                    break;
                case PersonalityType.Intelligence:
                    ExecuteSkillEnhancement(context);
                    break;
                case PersonalityType.Health:
                    ExecuteCageBlockade(context);
                    break;
                case PersonalityType.Sense:
                    ExecuteCageEnhancement(context);
                    break;
                case PersonalityType.Luck:
                    ExecuteScoreEnhancement(context);
                    break;

                default: break;
            }
        }

        void ExecuteMedalEnhancement(SkillExecuteContext context)
        {
            for (int i = 0; i < (int)SkillPower; i++)
            {
                context.Hub.MedalManager.GenerateMedal(m => MedalOwnership.Attach(m, context.Owner.ID));
            }
        }

        void ExecuteMedalRobbery(SkillExecuteContext context)
        {
            var effectPrefab = ResourceManager.GetResource<CageMedalRobbery>();

            var ownerCages = context.Hub.CageManager.AllCages
                .Where(cage =>
                {
                    var marker = cage.GetComponentInChildren<CageOwnershipMarker>();
                    return marker != null && marker.ID == context.Owner.ID;
                });

            foreach (var cage in ownerCages)
            {
                var effectInstance = Object.Instantiate(effectPrefab, cage.transform);
                var robberyComponent = effectInstance.GetComponent<CageMedalRobbery>();
                robberyComponent.Setup((int)SkillPower);
            }
        }
        void ExecuteCageBlockade(SkillExecuteContext context)
        {
            var blockadePrefab = ResourceManager.GetResource<CageBlockade>();

            var enemyCages = context.Hub.CageManager.AllCages
                .Where(cage =>
                {
                    var marker = cage.GetComponentInChildren<CageOwnershipMarker>();
                    return marker != null && marker.ID != context.Owner.ID;
                });

            float R(float value)
            {
                return Random.Range(-value, value);
            }

            const float x = 0.25f;
            const float y = 0.50f;
            foreach (var cage in enemyCages)
            {
                for (int i = 0; i < SkillPower; i++)
                {
                    var blocker = Object.Instantiate(blockadePrefab, cage.transform);
                    blocker.transform.Translate(new Vector2(R(x), Mathf.Abs(R(y))));
                }
            }
        }
        void ExecuteCageEnhancement(SkillExecuteContext context)
        {
            var ownerCages = context.Hub.CageManager.AllCages
                .Where(cage =>
                {
                    var marker = cage.GetComponentInChildren<CageOwnershipMarker>();
                    return marker != null && marker.ID == context.Owner.ID;
                });

            foreach (var cage in ownerCages)
            {
                CageEnhancement.LevelUp(cage, (int)SkillPower);
            }
        }
        void ExecuteSkillEnhancement(SkillExecuteContext context)
        {
            var targetSkill = context.SelectedSkills
                .Where(s => s != this)
                .GetRandomElementOrDefault();

            if (targetSkill == null)
            {
                // Used = false;
                return;
            }

            targetSkill.SkillLevel += (int)SkillPower;
        }
        void ExecuteScoreEnhancement(SkillExecuteContext context)
        {
            var ownerCages = context.Hub.CageManager.AllCages
                .Where(cage =>
                {
                    var marker = cage.GetComponentInChildren<CageOwnershipMarker>();
                    return marker != null && marker.ID == context.Owner.ID;
                });

            foreach (var cage in ownerCages)
            {
                var scoreAdder = cage.GetComponentInChildren<CageScoreAdder>();
                if (scoreAdder != null)
                {
                    scoreAdder.ScoreMultiplier *= (int)SkillPower;
                }
            }
        }
    }
}
