using System;
using System.Linq;
using System.Collections.Generic;
using MedalGame;
using UnityEngine;

namespace MantenseiNobel.Mouhitotsu
{
    [Serializable]
    public class Player
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

        [SerializeField] List<Personality> _personalities = new();
        public IReadOnlyList<Personality> Personalities => _personalities;
        public int SkillCount => Personalities.Count(x => !x.Used);
        public bool HasPersonality(PersonalityType skillType) => Personalities.Any(x => x.PersonalityType == skillType && !x.Used);
        public Player(string id)
        {
            this._id = id;
        }

        public bool UseSkill(int skillIndex, MedalGameReferenceHub hub)
        {
            if (skillIndex < 0 || skillIndex >= _personalities.Count)
            {
                Debug.LogWarning($"Invalid skill index: {skillIndex}");
                return false;
            }

            if (hub.GameManager.Status != GameStatus.Idle)
            {
                Debug.LogWarning("Skills can only be used before the game starts");
                return false;
            }

            var skill = _personalities[skillIndex];
            var context = new SkillExecuteContext(hub, this);
            skill.Execute(context);
            _personalities.RemoveAt(skillIndex);
            return true;
        }

        public static implicit operator string(Player player) => player._id;
    }
}
