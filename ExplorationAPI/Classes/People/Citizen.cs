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
                int racialModifier = 0;
                if (Race.StatModifiers.ContainsKey(pstat)) racialModifier = Race.StatModifiers[pstat];
                PrimaryStats[pstat] = new(random.Next(50, 200), racialModifier);
            }
            foreach (string dstat in listTool.DerivedStats)
            {
                DerivedStats[dstat] = new(0);
            }
            RefreshDerived();
            // Gives the company 1 known bit of information.
            PrimaryStats.ElementAt(random.Next(PrimaryStats.Count)).Value.Known = true;
            #endregion

            #region ConstructSkills
            Skills = new();
            double nSkills = listTool.SkillsList.Count;
            double startSkills = 5;
            double highskill = 1;
            double chance = 0;
            foreach (var kvp in listTool.SkillsList)
            {
                // This determines if its a starting skill and sets it higher
                // This line avoids deividing by zero
                if (nSkills > 0) chance = startSkills / nSkills; else chance = 1;
                if (random.NextDouble() < (chance))
                {
                    if (startSkills > 0) chance = highskill / startSkills; else chance = 0;
                    if (random.NextDouble() < (chance))
                    { 
                        highskill--;
                        Skills[kvp.Key] = new(random.Next(200,300), kvp.Value[0], kvp.Value[1]);
                    } else Skills[kvp.Key] = new(random.Next(100, 175), kvp.Value[0], kvp.Value[1]);
                    startSkills--;
                } else Skills[kvp.Key] = new(0, kvp.Value[0], kvp.Value[1]);
                nSkills--;
            }
            CalculateSkills();
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
            Dictionary<string, DerivedStat> derivedstats,
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
        public Dictionary<string, DerivedStat> DerivedStats { get; set; }
        public Dictionary<string, Attribute> Attributes { get; set; }
        public Dictionary<string,Modifier> Modifiers { get; set; }
        public Dictionary<string,Trait> Traits { get; set; }
        #endregion
    }
}
