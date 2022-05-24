using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FileTools;
using Users;
//using Newtonsoft.Json;
using Relation;

namespace Company
{
    //The object that holds everything about a company, should sit below a player account
    public partial class PlayerCompany
    {
        //Initialy building a company
        #region Constructor
        internal PlayerCompany(string name, Citizen master, List<Citizen> advisors, User user, ICitizenCache citizenCache, long time)
        {
            Relationships = new();
            if (advisors.Count != 7)
                throw new ArgumentException($"There are {advisors.Count} advisors in the list, there must be 7.");
            Name = name;
            id = Guid.NewGuid();
            UserId = user.id;
            Advisors = new();
            Recruits = new();
            EventStatus = new();
            TimeBlock = new();
            AddAdvisor(master, "master");
            //Sets the first 5 citizens in advisors to the other advisors
            for (int i = 0; i < 5; i++)
            {
                string advisorNumber = "advisor" + (i+1).ToString();
                AddAdvisor(advisors[i], advisorNumber);
            }
            //Sets the last 2 advisors to bench positions
            for (int i = 5; i < 7; i++)
            {
                string benchNumber = "bench" + (i-4).ToString();
                AddAdvisor(advisors[i], benchNumber);
            }

            //Create an initial pool of Recruits
            Recruits = new();
            for (int i = 0; i < 4; i++)
            {
                Citizen recruit = citizenCache.GetRandomCitizen();
                Recruits.Add("recruit" + (i+1).ToString(), recruit);
            }
            Skills = new();
            foreach (Skill skill in Enum.GetValues(typeof(Skill))) { Skills.Add(skill, new(0)); }
            UpdateCompanySkills();
        }

        [JsonConstructor]
        public PlayerCompany(
            string name,
            Guid Id,
            Dictionary<string,Citizen> advisors,
            Dictionary<string,Relationship> relationships,
            Dictionary<Skill, CompanySkillBlock> skills,
            Guid userId,
            Dictionary<string, Citizen> recruits,
            EventStatus eventStatus,
            TimeBlock timeBlock)
        {
            Name = name;
            id = Id;
            Advisors = advisors;
            Relationships = relationships;
            Skills = skills;
            UserId = UserId;
            Recruits= recruits;
            EventStatus = eventStatus;
            TimeBlock = timeBlock;
        }


        #endregion

        #region Dictionaries and Properties
        public string Name { get; set; }
        public Guid id { get; set; }
        public Guid UserId { get; set; }
        public Dictionary<string, Citizen> Advisors { get; set; }
        public Dictionary<string, Relationship> Relationships { get; set; }
        public Dictionary<Skill, CompanySkillBlock> Skills { get; set; }
        public Dictionary<string, Citizen> Recruits { get; set; }
        public EventStatus EventStatus { get; set; }
        public TimeBlock TimeBlock { get; set; }
        #endregion
    }
}
