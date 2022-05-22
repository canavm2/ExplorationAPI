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

            foreach (Stat pstat in Enum.GetValues(typeof(Stat)))
                {
                int racialModifier = 0;
                if (Race.StatModifiers.ContainsKey(pstat)) racialModifier = Race.StatModifiers[pstat];
                PrimaryStats[pstat] = new(random.Next(50, 200), racialModifier);
            }
            foreach (DerivedStat dstat in Enum.GetValues(typeof(DerivedStat)))
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
            foreach (Skill skill in Enum.GetValues(typeof(Skill)))
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
                        Skills[skill] = new(random.Next(200,300), listTool.SkillsList[skill][0], listTool.SkillsList[skill][1]);
                    } else Skills[skill] = new(random.Next(100, 175), listTool.SkillsList[skill][0], listTool.SkillsList[skill][1]);
                    startSkills--;
                } else Skills[skill] = new(0, listTool.SkillsList[skill][0], listTool.SkillsList[skill][1]);
                nSkills--;
            }
            CalculateSkills();
            #endregion

            #region ConstructAttributes
            Attributes = new();
            foreach (Attribute attribute in Enum.GetValues(typeof(Attribute))) Attributes.Add(attribute, new AttributeBlock());
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
            Dictionary<Skill, SkillBlock> skills,
            Dictionary<Stat, StatBlock> primarystats,
            Dictionary<DerivedStat, DerivedStatBlock> derivedstats,
            Dictionary<string, Modifier> modifiers,
            Dictionary<Attribute, AttributeBlock> attributes,
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
        public Dictionary<Skill, SkillBlock> Skills { get; set; }
        public Race Race { get; set; }
        public Dictionary<Stat, StatBlock> PrimaryStats { get; set; }
        public Dictionary<DerivedStat, DerivedStatBlock> DerivedStats { get; set; }
        public Dictionary<Attribute, AttributeBlock> Attributes { get; set; }
        public Dictionary<string,Modifier> Modifiers { get; set; }
        public Dictionary<string,Trait> Traits { get; set; }
        #endregion
    }
}
