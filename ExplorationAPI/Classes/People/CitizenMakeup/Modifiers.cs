using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace People
{
    public class Modifier
    {
        #region Constructors
        public Modifier(string name, string description, int value, bool temporary = false, int duration = -1)
        {
            Name = name;
            Description = description;
            Value = value;
            Temporary = temporary;
            if (temporary)
            {
                Duration = duration;
            }
            Expiration = new();

        }


        [JsonConstructor]
        public Modifier(string name, string description, DateTime expiration, int value, bool temporary, int duration)
        {
            Name = name;
            Description = description;
            Temporary = temporary;
            Value = value;
            Duration = duration;
            Expiration = expiration;
        }
        #endregion

        public string Name { get; internal set; }
        public string Description { get; internal set; }
        public int Value { get; internal set; }
        public bool Temporary { get; internal set; }
        public int Duration { get; internal set; }
        public DateTime Expiration { get; set; }
    }
    public class StatModifier : Modifier
    {
        public StatModifier(string name, string description, int value, bool temporary, int duration, Stat modifiedStat) : base(name, description, value, temporary, duration)
        {
            ModifiedStat = modifiedStat;
        }
        public StatModifier(string name, string description, DateTime expiration, int value, bool temporary, int duration, Stat modifiedStat)
            : base(name, description, expiration, value, temporary, duration)
        {
            ModifiedStat = modifiedStat;
        }

        public Stat ModifiedStat { get; internal set; }
    }

    public class SkillModifier : Modifier
    {
        public SkillModifier(string name, string description, int value, bool temporary, int duration, Skill modifiedSkill) : base(name, description, value, temporary, duration)
        {
            ModifiedSkill = modifiedSkill;
        }
        public SkillModifier(string name, string description, DateTime expiration, int value, bool temporary, int duration, Skill modifiedSkill)
            : base(name, description, expiration, value, temporary, duration)
        {
            ModifiedSkill = modifiedSkill;
        }

        public Skill ModifiedSkill { get; internal set; }
    }
    public class AttributeModifier : Modifier
    {
        public AttributeModifier(string name, string description, int value, bool temporary, int duration, Attribute modifiedAttribute) : base(name, description, value, temporary, duration)
        {
            ModifiedAttribute = modifiedAttribute;
        }
        public AttributeModifier(string name, string description, DateTime expiration, int value, bool temporary, int duration, Attribute modifiedAttribute)
            : base(name, description, expiration, value, temporary, duration)
        {
            ModifiedAttribute = modifiedAttribute;
        }

        public Attribute ModifiedAttribute { get; internal set; }
    }

}
