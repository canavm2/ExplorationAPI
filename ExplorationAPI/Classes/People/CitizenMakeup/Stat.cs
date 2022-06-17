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
            _unmod = unmod;
            _max = random.Next(250,350);
            _racialModifier = racialModifier;
            Known = false;
            _modifiers = new();
        }

        [JsonConstructor]
        public StatBlock(int _Unmod, int _Max, int _RacialModifier, Boolean known, Dictionary<string, StatModifier> _Modifiers)
        {
            Known = known;
            _max = _Max;
            _unmod = _Unmod;
            _racialModifier = _RacialModifier;
            _modifiers = _Modifiers;
        }
        #endregion

        #region Dictionaries and Properties
        public bool Known { get; set; }

        public int _max { get; set; }

        public int _unmod { get; set; }

        public int _racialModifier { get; set; }

        public Dictionary<string, StatModifier> _modifiers { get; set; }
        #endregion

        #region Methods
        public void ApplyModifier(StatModifier modifier, PlayerCompany company)
        {
            // Sets temporary modifiers expiration based on the companies current time and the duration of the modifier.
            if (modifier.Temporary) modifier.Expiration = company.TimeBlock.CurrentTime.Add(TimeSpan.FromSeconds(modifier.Duration));

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
        #endregion
    }

}
