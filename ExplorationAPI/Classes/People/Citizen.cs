using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FileTools;
//using Newtonsoft.Json;

namespace People
{
    public partial class Citizen
    {
        #region Constructors
        //Builds a random citizen
        public Citizen(string name, string gender, int age = 0)
        {
            Random random = new();
            if (age == 0)
                age = random.Next(15, 40);
            Name = name;
            Age = age;
            List<string> genders = new() { "male", "female", "non-binary" };
            if (genders.Contains(gender))
                Gender = gender;
            else
                Gender = "non-binary";
            id = Guid.NewGuid();
            Skills = new();
            Modifiers = new();
            Traits = new();

            #region ConstructStats
            ListTool listTool = new ListTool();
            PrimaryStats = new();
            DerivedStats = new();
            Modifiers = new();

            foreach (string pstat in listTool.PrimaryStats)
            {
                PrimaryStats[pstat] = new(random.Next(10, 30));
            }
            foreach (string dstat in listTool.DerivedStats)
            {
                DerivedStats[dstat] = new(0);
            }
            RefreshDerived();
            #endregion

            #region ConstructAttributes
            Attributes = new();
            foreach (string attribute in listTool.Attributes)
                Attributes.Add(attribute, new Attribute());
            #endregion

            #region ConstructTraits
            //TODO Actually Construct Traits
            Traits = new();
            #endregion
        }

        [JsonConstructor]
        public Citizen(
            string name,
            string gender,
            Guid Id, int age,
            Skills skills,
            Dictionary<string, Stat> primarystats,
            Dictionary<string, Stat> derivedstats,
            Dictionary<string, Modifier> modifiers,
            Dictionary<string, Attribute> attributes,
            Dictionary<string, Trait> traits
            )
        {
            Name = name;
            Gender = gender;
            id = Id;
            Age = age;
            Skills = skills;
            PrimaryStats = primarystats;
            DerivedStats = derivedstats;
            Modifiers = modifiers;
            Attributes = attributes;
            Traits = traits;
        }
        #endregion

        #region Dictionaries and Properties

        public Guid id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public Skills Skills { get; set; }
        public Dictionary<string, Stat> PrimaryStats { get; set; }
        public Dictionary<string, Stat> DerivedStats { get; set; }
        public Dictionary<string, Attribute> Attributes { get; set; }
        public Dictionary<string,Modifier> Modifiers { get; set; }
        public Dictionary<string,Trait> Traits { get; set; }
        #endregion

        #region Subclasses
        //Objects to contain Full and Unmodified values of things like skills
        public class Attribute
        {
            #region Constructors
            public Attribute()
            {
                Full = 10;
                Unmodified = 10;
            }

            [JsonConstructor]
            public Attribute(int full, int unmodified)
            {
                Full = full;
                Unmodified = unmodified;
            }
            #endregion
            public int Full { get; set; }
            public int Unmodified { get; set; }
        }
        public class Stat
        {
            public Stat(int unmod)
            {
                Unmodified = unmod;
                Full = unmod;
            }

            [JsonConstructor]
            public Stat(int unmodified, int full)
            {
                Unmodified = unmodified;
                Full = full;
            }
            public int Unmodified { get; set; }
            public int Full { get; set; }
        }
        public class Trait
        {
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

            public string Name { get; set; }
            public  int Tier { get; set; }
            public Boolean Known { get; set; }
            public  List<Modifier> Modifiers { get; set; }

            public string Summary()
            {
                string returnstring = $"\nTrait: {Name}." +
                    $"\nModifiers:\n";
                foreach (Modifier modifier in Modifiers)
                    returnstring += modifier.Summary();
                return returnstring;
            }
        }
        #endregion
    }
}
