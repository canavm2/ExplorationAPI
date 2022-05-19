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
        public Citizen(string name, string gender, string race = "Human", int age = 0)
        {
            Random random = new();
            if (age == 0)
                age = random.Next(15, 40);
            Race = new Race(race);
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
                PrimaryStats[pstat] = new(random.Next(50, 200));
            }
            foreach (string dstat in listTool.DerivedStats)
            {
                DerivedStats[dstat] = new(0);
            }
            RefreshDerived();
            #endregion

            #region ConstructSkills
            Skills = new();
            int nSkills = listTool.SkillsList.Count;
            int startSkills = 5;
            int highskill = 1;
            foreach (var kvp in listTool.SkillsList)
            {
                // This determines if its a starting skill and sets it higher
                if (random.NextDouble() < (startSkills / nSkills))
                {
                    startSkills--;
                    if (random.NextDouble() < (highskill / startSkills)){
                        highskill--;
                        Skills[kvp.Key] = new(random.Next(300,400), kvp.Value[0], kvp.Value[1]);
                    } else Skills[kvp.Key] = new(random.Next(150, 250), kvp.Value[0], kvp.Value[1]);
                }
                Skills[kvp.Key] = new(0, kvp.Value[0], kvp.Value[1]);
                nSkills--;
            }
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
            Race race,
            Guid Id, int age,
            Dictionary<string, Skill> skills,
            Dictionary<string, Stat> primarystats,
            Dictionary<string, Stat> derivedstats,
            Dictionary<string, Modifier> modifiers,
            Dictionary<string, Attribute> attributes,
            Dictionary<string, Trait> traits
            )
        {
            Name = name;
            Gender = gender;
            Race = race;
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
        public Dictionary<string, Skill> Skills { get; set; }
        public Race Race { get; set; }
        public Dictionary<string, Stat> PrimaryStats { get; set; }
        public Dictionary<string, Stat> DerivedStats { get; set; }
        public Dictionary<string, Attribute> Attributes { get; set; }
        public Dictionary<string,Modifier> Modifiers { get; set; }
        public Dictionary<string,Trait> Traits { get; set; }
        #endregion
    }
}
