using System.Text.Json.Serialization;

namespace People
{
    public class Race
    {
        #region Constructors
        public Race(string name)
        {
            Name = name;
            StatModifiers = new();
            if (name == "Southernman")
            {
                StatModifiers[Stat.INT] = 20;
            }
            else if (name == "Northman")
            {
                StatModifiers[Stat.STR] = 20;
            }
            else
            {
                Name = "Human"; //Default in case misspelled.
                StatModifiers[Stat.LDR] = 20;
            }
        }
        [JsonConstructor]
        public Race(string name, Dictionary<Stat, int> statModifiers)
        {
            Name = name;
            StatModifiers = statModifiers;
        }
        #endregion

        public string Name { get; set; }
        public Dictionary<Stat, int> StatModifiers { get; set; }
    }
}
