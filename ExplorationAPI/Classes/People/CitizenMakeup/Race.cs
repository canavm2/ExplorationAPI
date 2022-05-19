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
                StatModifiers["INT"] = 20;
            }
            else if (name == "Northman")
            {
                StatModifiers["STR"] = 20;
            }
            else
            {
                Name = "Human"; //Default in case misspelled.
                StatModifiers["LDR"] = 20;
            }
        }
        [JsonConstructor]
        public Race(string name, Dictionary<string, int> statModifiers)
        {
            Name = name;
            StatModifiers = statModifiers;
        }
        #endregion

        public string Name;
        public Dictionary<string, int> StatModifiers;
    }
}
