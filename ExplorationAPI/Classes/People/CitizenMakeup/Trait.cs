using System.Text.Json.Serialization;

namespace People
{
    public class Trait
    {
        #region Constructors
        public Trait(string name, int tier, List<Modifier> modifiers)
        {
            Name = name;
            Tier = tier;
            Modifiers = modifiers;
            Known = false;
        }
        [JsonConstructor]
        public Trait(string name, int tier, List<Modifier> modifiers, Boolean known)
        {
            Name = name;
            Tier = tier;
            Modifiers = modifiers;
            Known = known;
        }
        #endregion


        public string Name { get; set; }
        public int Tier { get; set; }
        public Boolean Known { get; set; }
        public List<Modifier> Modifiers { get; set; }

        //public string Summary()
        //{
        //    string returnstring = $"\nTrait: {Name}." +
        //        $"\nModifiers:\n";
        //    foreach (Modifier modifier in Modifiers)
        //        returnstring += modifier.Summary();
        //    return returnstring;
        //}
    }
}