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
    //public class CitizenSkills
    //{
    //    //Skills is maintained as a seperate object unlike stats and attributes because it exists at both the citizen and company level
    //    #region Constructors

    //    //Takes type="company" parameter to produce a set of company skills
    //    //public CitizenSkills()
    //    //{
    //    //    Skills = new();
    //    //    ListTool listTool = new ListTool();
    //    //    Random random = new();
    //    //    // Reads through each skill in the list and makes a skill for it, setting it to a value and saving the appropriate stats as the base stats
    //    //    foreach (var kvp in listTool.VocSkillsList) Skills[kvp.Key] = new(0, kvp.Value[0], kvp.Value[1]);//random.Next(0, 10);

    //    //    if (type != "company")
    //    //    {
    //    //        List<string> tempVocSkillsList = listTool.VocSkillsList.Keys.ToList();
    //    //        int index = random.Next(tempVocSkillsList.Count);
    //    //        string highskill = tempVocSkillsList[index];
    //    //        tempVocSkillsList.RemoveAt(index);
    //    //        VocSkill[highskill] = new(random.Next(30, 40));
    //    //        for (int i = 0; i < 4; i++)
    //    //        {
    //    //            index = random.Next(tempVocSkillsList.Count);
    //    //            highskill = tempVocSkillsList[index];
    //    //            tempVocSkillsList.RemoveAt(index);
    //    //            VocSkill[highskill] = new(random.Next(15, 25));
    //    //        }
    //    //    }


    //    //}


    //    [JsonConstructor]
    //    public CitizenSkills(Dictionary<string, Skill> skills, Dictionary<string, Skill> expskill)
    //    {
    //        Skills = skills;
    //    }
    //    #endregion

    //    #region Dictionaries and Properties
    //    public Dictionary<string, Skill> Skills { get; set; }
    //    #endregion

    //    #region Methods
    //    public string Describe()
    //    {
    //        //Iterates over all the Skills, and provides a string that describes it
    //        string skillsDesc = "";
    //        foreach (KeyValuePair<string, Skill> skill in Skills)
    //        {
    //            if (skill.Value.Full > 0)
    //            {
    //                string tempDesc = $"{skill.Key}: {skill.Value.Full.ToString()}\n";
    //                skillsDesc += tempDesc;
    //            }
    //        }
    //        string description =
    //            $"Vocational Skills:\n" + skillsDesc;
    //        return description;
    //    }
    //    #endregion

    //}

    public class Skill
    {
        public Skill(int unmod, string pstat, string sstat)
        {
            Full = unmod;
            Unmodified = unmod;
            pStat = pstat;
            sStat = sstat;
        }

        [JsonConstructor]
        public Skill(int full, int unmodified, string pstat, string sstat)
        {
            Full = full;
            Unmodified = unmodified;
            pStat = pstat;
            sStat = sstat;
        }
        public int Full { get; set; }
        public int Unmodified { get; set; }
        public string pStat { get; set; }
        public string sStat { get; set; }
    }

}
