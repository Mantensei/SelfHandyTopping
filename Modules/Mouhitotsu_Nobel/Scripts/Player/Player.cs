using System.Linq;
using System.Collections.Generic;
using MedalGame;
using UnityEngine;

namespace MantenseiNovel.Mouhitotsu
{
    [CreateAssetMenu(fileName = "Player", menuName = "Mouhitotsu/Player")]
    public class Player : ScriptableObject
    {
        [SerializeField] string _id;
        public string ID => _id;

        public bool Alive { get; set; } = true;

        [SerializeField] Color _color = Color.gray;
        public Color Color => _color;
        [SerializeField] string _mark = "-";
        public string Mark => _mark;
        [SerializeField] string _name = "none";
        public string Name => _name;

        List<Personality> _personalities = new();
        public IReadOnlyList<Personality> Personalities => _personalities;

        public void AddPersonality(Personality personality)
        {
            _personalities.Add(personality);
        }

        public int SkillCount => Personalities.Count(x => !x.Used);
        public int UsedSkillCount => Personalities.Count(x => x.Used);
        public bool HasPersonality(PersonalityType skillType) => Personalities.Any(x => x.PersonalityType == skillType && !x.Used);

        public static implicit operator string(Player player) => player._id;
    }
}
