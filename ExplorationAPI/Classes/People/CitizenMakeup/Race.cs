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
                StatModifiers["INT"] = 5;
            }
            else if (name == "Northman")
            {
                StatModifiers["STR"] = 5;
            }
            else
            {
                Name = "Human"; //Default in case misspelled.
                StatModifiers["LDR"] = 5;
            }
        }
        [JsonConstructor]
        public Race(string name, Dictionary<string, int> statModifiers)
        {
            Name = name;
            StatModifiers = statModifiers;
        }
        #endregion

        string Name;
        Dictionary<string, int> StatModifiers;
    }
}
