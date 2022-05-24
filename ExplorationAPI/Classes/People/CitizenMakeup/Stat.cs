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
            _modifiers = new();
        }

        [JsonConstructor]
        public StatBlock(int unmod, int max, int racialModifier, Boolean known, Dictionary<string, StatModifier> modifiers)
        {
            Unmod = unmod;
            Max = max;
            RacialModifier = racialModifier;
            Known = known;
            _modifiers = modifiers;
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

        private int _totalModifiedValue {
            get {if (_modifiers.Count > 0) {  // Only checks if there are any Modifiers, else returns 0.
                    int value = 0;
                    List<string> expired = new();

                    // Checks each modifier to see if its expired, if not expired it adds it up; if expired, deletes it.
                    foreach (StatModifier modifier in _modifiers.Values)
                    {
                        if (modifier.Expiration > DateTime.UtcNow) expired.Add(modifier.Name);
                        else value += modifier.Value;
                    }
                    foreach (String name in expired) RemoveModifier(name);
                    return value;
                }
                else return 0;
            }
        }

        private Dictionary<string, StatModifier> _modifiers { get; set; }

        public void ApplyModifier(StatModifier modifier, PlayerCompany company)
        {
            // Sets temporary modifiers expiration based on the companies current time and the duration of the modifier.
            if (modifier.Temporary) modifier.Expiration = DateTime.UtcNow.Add(TimeSpan.FromSeconds(modifier.Duration));

            // Checks to see if the modifier is in the Modifiers list already.
            if (_modifiers.ContainsKey(modifier.Name))
            {
                StatModifier currentModifier = _modifiers[modifier.Name];
                // If the current modifier is bigger, returns without adding the new modifier.  Otherwise replaces the current with the new
                if ((modifier.Value <= currentModifier.Value)) return;
                else _modifiers[modifier.Name] = modifier;
            }
            // If its not in the Modifiers list, it adds the modifier to the list.
            else
            {
                _modifiers.Add(modifier.Name, modifier);
            }
        }
        public void RemoveModifier(string name)
        {
            if (_modifiers.ContainsKey(name)) _modifiers.Remove(name);
            else throw new Exception("No modifier to remove.");
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
