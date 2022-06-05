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
        internal PlayerCompany(string name, Citizen master, List<Citizen> advisors, User user, ICitizenCache citizenCache)
        {
            if (advisors.Count != 7)
                throw new ArgumentException($"There are {advisors.Count} advisors in the list, there must be 7.");
            Name = name;
            id = Guid.NewGuid();
            UserId = user.id;
            Members = new();
            EventStatus = new();
            TimeBlock = new();
            master.AdvisorBlock.Master = true;
            master.AdvisorBlock.Advisor = true;
            AddMember(master, "master");
            //Sets the first 5 citizens in advisors to the other advisors
            for (int i = 0; i < 5; i++)
            {
                string advisorNumber = "advisor" + (i+1).ToString();
                advisors[i].AdvisorBlock.Advisor = true;
                AddMember(advisors[i], advisorNumber);
            }
            //Sets the last 2 advisors to bench positions
            for (int i = 5; i < 7; i++)
            {
                string benchNumber = "bench" + (i-4).ToString();
                AddMember(advisors[i], benchNumber);
            }
                        
            Skills = new();
            foreach (Skill skill in Enum.GetValues(typeof(Skill))) { Skills.Add(skill, new(0)); }
            UpdateCompanySkills();
        }

        [JsonConstructor]
        public PlayerCompany(
            string name,
            Guid Id,
            Dictionary<string, Citizen> members,
            Dictionary<Skill, CompanySkillBlock> skills,
            Guid userId,
            EventStatus eventStatus,
            TimeBlock timeBlock)
        {
            Name = name;
            id = Id;
            Members = members;
            Skills = skills;
            UserId = userId;
            EventStatus = eventStatus;
            TimeBlock = timeBlock;
        }


        #endregion

        #region Dictionaries and Properties
        public string Name { get; set; }
        public Guid id { get; set; }
        public Guid UserId { get; set; }

        private Dictionary<string, Citizen> _members;
        public Dictionary<string, Citizen> Members { get { return _members; } internal set { _members = value; } }
        public List<Citizen> Advisors
        {
            get
            {
                List<Citizen> _advisors = new();
                foreach (var member in _members.Values)
                {
                    if (member.AdvisorBlock.Advisor) _advisors.Add(member);
                }
                return _advisors;
            }
        }
        public List<Citizen> Vanguard
        {
            get
            {
                List<Citizen> _vanguard = new();
                foreach (var member in _members.Values)
                {
                    if (member.AdvisorBlock.Vanguard) _vanguard.Add(member);
                }
                return _vanguard;
            }
        }
        public List<Citizen> Bench
        {
            get
            {
                List<Citizen> _bench = new();
                foreach (var member in _members.Values)
                {
                    if (!member.AdvisorBlock.Advisor & !member.AdvisorBlock.Vanguard) _bench.Add(member);
                }
                return _bench;
            }
        }
        public Citizen Master
        {
            get
            {
                // TODO fix the possibility of returning no master. and unnecessary creation of citizen master.
                Citizen _master = new("must fix", "male");
                foreach (var citizen in _members.Values)
                {
                    if (citizen.AdvisorBlock.Master)
                    {
                        _master = citizen;
                    }
                }
                return _master;
            }
        }

        public Dictionary<Skill, CompanySkillBlock> Skills { get; set; }
        public EventStatus EventStatus { get; set; }
        public TimeBlock TimeBlock { get; set; }
        #endregion
    }
}
