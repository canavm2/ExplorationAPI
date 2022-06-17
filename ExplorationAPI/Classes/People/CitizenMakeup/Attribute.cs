using System.Text.Json.Serialization;

namespace People
{
    public enum Attribute
    {
        Health,
        Endurance,
        Happiness,
        Motivation,
        Psyche
    }
    public class AttributeBlock
    {
        #region Constructors
        public AttributeBlock()
        {
            _unmod = 100;
            _max = 300;
            _modifiers = new();
        }

        [JsonConstructor]
        public AttributeBlock(int _Unmod, int _Max, Dictionary<string, AttributeModifier> _Modifiers)
        {
            _unmod = _Unmod;
            _max = _Max;
            _modifiers = _Modifiers;
        }
        #endregion
        public int _unmod { get; set; }
        public int _max { get; set; }
        public Dictionary<string, AttributeModifier> _modifiers { get; set; }

        public void ApplyModifier(AttributeModifier modifier, PlayerCompany company)
        {
            // Sets temporary modifiers expiration based on the companies current time and the duration of the modifier.
            if (modifier.Temporary) modifier.Expiration = company.TimeBlock.CurrentTime.Add(TimeSpan.FromSeconds(modifier.Duration));

            // Checks to see if the modifier is in the Modifiers list already.
            if (_modifiers.ContainsKey(modifier.Name))
            {
                AttributeModifier currentModifier = _modifiers[modifier.Name];
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

}
