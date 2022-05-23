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
        public Modifier(string name, string description, int value, bool temporary = false, int duration = 0)
        {
            Name = name;
            Description = description;
            Value = value;
            Temporary = temporary;
            Duration = duration;
            Expiration = 0;
        }


        [JsonConstructor]
        public Modifier(string name, string description, int expiration, int value, bool temporary = false, int duration = 0)
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
        public int Expiration { get; set; }
    }
    public class StatModifier : Modifier
    {
        public StatModifier(string name, string description, int value, bool temporary, int duration, Stat modifiedStat) : base(name, description, value, temporary, duration)
        {
            ModifiedStat = modifiedStat;
        }
        public StatModifier(string name, string description, int value, int expiration, bool temporary, int duration, Stat modifiedStat)
            : base(name, description, value, temporary, duration)
        {
            ModifiedStat = modifiedStat;
        }

        public Stat ModifiedStat { get; internal set; }
    }
}
