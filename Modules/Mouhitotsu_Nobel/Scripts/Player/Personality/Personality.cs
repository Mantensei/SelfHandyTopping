using MedalGame;
using UnityEngine;

namespace MantenseiNobel.Mouhitotsu
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

        public SkillExecuteContext(MedalGameReferenceHub hub, Player owner)
        {
            Hub = hub;
            Owner = owner;
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

        public PersonalityType PersonalityType => _personalityType;
        public string Name => _name;
        public string SkillName => _skillName;
        public string SkillDescription => _skillDescription;
        public float SkillPower => _skillPower;
        public bool Used { get; private set; } = false;

        public void Execute(SkillExecuteContext context)
        {
            if (Used) return;
            Used = true;

            switch (_personalityType)
            {
                case PersonalityType.Kindness:
                    ExecuteMedalEnhancement(context);
                    break;
                case PersonalityType.Money:
                    ExecuteMedalRobbery(context);
                    break;
                case PersonalityType.Intelligence:
                    ExecuteCageBlockade(context);
                    break;
                case PersonalityType.Health:
                    ExecuteCageEnhancement(context);
                    break;
                case PersonalityType.Sense:
                    ExecuteSkillEnhancement(context);
                    break;
                case PersonalityType.Luck:
                    ExecuteScoreEnhancement(context);
                    break;

                default: break;
            }
        }

        void ExecuteMedalEnhancement(SkillExecuteContext context) { }
        void ExecuteMedalRobbery(SkillExecuteContext context) { }
        void ExecuteCageBlockade(SkillExecuteContext context) { }
        void ExecuteCageEnhancement(SkillExecuteContext context) { }
        void ExecuteSkillEnhancement(SkillExecuteContext context) { }
        void ExecuteScoreEnhancement(SkillExecuteContext context) { }
    }
}
