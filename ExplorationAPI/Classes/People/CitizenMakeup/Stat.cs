using System.Text.Json.Serialization;

namespace People
{
    public enum Stat
    {
        STR,
        AGI,
        CON,
        INT,
        WIS,
        PER,
        CHA,
        LDR,
        WIL
    }
    public enum DerivedStat
    {
        PHYS,
        MNTL,
        SOCL
    }
    public class StatBlock
    {
        #region Constructors
        public StatBlock(int unmod, int racialModifier)
        {
            Random random = new Random();
            Unmod = unmod;
            Max = random.Next(250,350);
            RacialModifier = racialModifier;
            Known = false;
            _totalModifiedValue = 0;
            Modifiers = new();
        }

        [JsonConstructor]
        public StatBlock(int unmod, int max, int racialModifier, Boolean known, Dictionary<string, StatModifier> modifiers)
        {
            Unmod = unmod;
            Max = max;
            RacialModifier = racialModifier;
            Known = known;
            Modifiers = modifiers;
        }
        #endregion

        public bool Known { get; set; }

        private int _max;
        public int Max {
            get { return _max; }
            internal set { _max = value; }
        }
        private int _unmod;
        public int Unmod {
            get { return _unmod; }
            internal set {_unmod = value; }
        }
        public int Full {
            get { return _unmod + _racialModifier + _totalModifiedValue; }
        }
        private int _racialModifier;
        public int RacialModifier {
            get { return _racialModifier; }
            set {_racialModifier = value; }
        }
        private int _totalModifiedValue;

        public Dictionary<string, StatModifier> Modifiers { get; internal set; }

        public void ApplyModifier(StatModifier modifier)
        {
            if (Modifiers.ContainsKey(modifier.Name))
            {

            }
            Modifiers.Add(modifier.Name, modifier);
            _totalModifiedValue += modifier.Value;
        }
        public void RemoveModifier(string name)
        {

        }
    }

    public class DerivedStatBlock
    {
        #region Constructors
        [JsonConstructor]
        public DerivedStatBlock(int full)
        {
            Full = full;
        }
        #endregion
        public int Full { get; set; }
    }

}
