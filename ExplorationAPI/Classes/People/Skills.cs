using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
//using Newtonsoft.Json;
using FileTools;

namespace People
{
    public class Skills
    {
        //Skills is maintained as a seperate object unlike stats and attributes because it exists and both the citizen and company level
        #region Constructors

        //Takes type="company" parameter to produce a set of company skills
        public Skills(string type = "citizen")
        {
            VocSkill = new();
            ExpSkill = new();
            ListTool listTool = new ListTool();
            Random random = new();
            foreach (string skill in listTool.VocSkillsList)
            {
                if (type == "company") VocSkill[skill] = new(0);
                else VocSkill[skill] = new(0);//random.Next(0, 10);
            }

            if (type != "company")
            {
                List<string> tempVocSkillsList = listTool.VocSkillsList;
                int index = random.Next(tempVocSkillsList.Count);
                string highskill = tempVocSkillsList[index];
                tempVocSkillsList.RemoveAt(index);
                VocSkill[highskill] = new(random.Next(30, 40));
                for (int i = 0; i < 4; i++)
                {
                    index = random.Next(tempVocSkillsList.Count);
                    highskill = tempVocSkillsList[index];
                    tempVocSkillsList.RemoveAt(index);
                    VocSkill[highskill] = new(random.Next(15, 25));
                }
            }

            //Sets all the experiential skills to 0
            foreach (string skill in listTool.ExpSkillsList)
            {
                if (type == "company") ExpSkill[skill] = new(0);
                else ExpSkill[skill] = new(0);// random.Next(0, 10);
            }
        }


        [JsonConstructor]
        public Skills(Dictionary<string, Skill> vocskill, Dictionary<string, Skill> expskill)
        {
            VocSkill = vocskill;
            ExpSkill = expskill;
        }
        #endregion

        #region Dictionaries and Properties
        public Dictionary<string, Skill> VocSkill { get; set; }
        public Dictionary<string, Skill> ExpSkill { get; set; }
        #endregion

        #region Methods
        public string Describe()
        {
            //Iterates over all the Skills, and provides a string that describes it
            string vocDesc = "";
            foreach (KeyValuePair<string, Skill> skill in VocSkill)
            {
                if(skill.Value.Full > 0)
                {
                    string tempDesc = $"{skill.Key}: {skill.Value.Full.ToString()}\n";
                    vocDesc += tempDesc;
                }
            }
            string expDesc = "";
            foreach (KeyValuePair<string, Skill> skill in ExpSkill)
            {
                if (skill.Value.Full > 0)
                {
                    string tempDesc = $"{skill.Key}: {skill.Value.Full.ToString()}\n";
                    expDesc += tempDesc;
                }
            }
            string description =
                $"Vocational Skills:\n" +
                vocDesc +
                $"\nExperiential Skills:\n" +
                expDesc;
            return description;
        }
        #endregion

    }

    #region subclasses
    public class Skill
    {
        public Skill(int unmod)
        {
            Full = unmod;
            Unmodified = unmod;
        }

        [JsonConstructor]
        public Skill(int full, int unmodified)
        {
            Full=full;
            Unmodified=unmodified;
        }
        public int Full { get; set; }
        public int Unmodified { get; set; }
    }

    #endregion
}
