using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using People;
using FileTools;
using Users;
//using Newtonsoft.Json;
using Relation;

namespace Company
{
    public partial class PlayerCompany
    {
        #region Methods
        public string Describe()
        {
            string advisorDescription = "";
            foreach (KeyValuePair<string, Citizen> advisor in Advisors)
            {
                if (advisor.Key.Contains("advisor"))
                    advisorDescription += $"{advisor.Value.Name}\n";
            }
            string benchDescription = "";
            foreach (KeyValuePair<string, Citizen> advisor in Advisors)
            {
                if (advisor.Key.Contains("bench"))
                    benchDescription += $"{advisor.Value.Name}\n";
            }
            string companyDescription =
                $"The company's name is: {Name}.\n" +
                $"ID: {id}\n\n" +
                $"The company master is {Advisors["master"].Name}.\n\n" +
                $"The company advisors are:\n" +
                advisorDescription +
                $"\nThese advisors are on the bench:\n" +
                benchDescription +
                $"\nThe company skills are:\n" +
                Skills.Describe()
                ;
            return companyDescription;
        }

        //Ensure all the skills are up to date
        internal void UpdateCompanySkills()
        {
            foreach (KeyValuePair<string, Skill> kvp in Skills.VocSkill)
            {
                UpdateCompanySkill(kvp.Key, "voc");
            }
            foreach (KeyValuePair<string, Skill> kvp in Skills.ExpSkill)
            {
                UpdateCompanySkill(kvp.Key, "exp");
            }
        }
        //Update a single skill
        internal void UpdateCompanySkill(string skill, string type)
        {
            if (type == "voc")
            {
                List<int> skillvalues = new();
                foreach (Citizen citizen in Advisors.Values)
                {
                    skillvalues.Add(citizen.Skills.VocSkill[skill].Full);
                }
                skillvalues.Sort();
                skillvalues.Reverse();
                Skills.VocSkill[skill] = new((skillvalues[0] + skillvalues[1]) / 2);
            }
            else if (type == "exp")
            {
                List<int> skillvalues = new();
                foreach (Citizen citizen in Advisors.Values)
                {
                    skillvalues.Add(citizen.Skills.ExpSkill[skill].Full);
                }
                skillvalues.Sort();
                skillvalues.Reverse();
                Skills.ExpSkill[skill] = new((skillvalues[0] + skillvalues[1]) / 2);
            }
            else throw new Exception($"type must be voc or exp, not: {type}");
        }

        //Add an advisor to an EMPTY role, rarely used, except initial construction and vacant roles
        internal void AddAdvisor(Citizen citizen, string role)
        {
            //TODO verify the role is acceptible
            Advisors[role] = citizen;
            foreach (Citizen advisor in Advisors.Values)
            {
                if (advisor.id != citizen.id)
                {
                    Relationship relationship = new Relationship(citizen, advisor);
                    Relationships[relationship.id] = relationship;
                }
            }
        }

        //Add an advisor to an occupied role, most common, saves the old citizen to the citizen cache
        public void ReplaceAdvisor(Citizen citizen, string role, CitizenCache citizencache, RelationshipCache relationshipcache)
        {
            if (!Advisors.ContainsKey(role)) throw new Exception($"No citizen to replace in role: {role}.");
            Citizen replacedCitizen = Advisors[role];
            Advisors[role] = citizen;
            UpdateRelationships(relationshipcache);
            citizencache.CacheCitizen(replacedCitizen);
            UpdateCompanySkills();
        }

        public string CreateRelationshipId(string citId1, string citId2)
        {
            string Id1;
            string Id2;
            Guid guid1 = new Guid(citId1);
            Guid guid2 = new Guid(citId2);
            int compare = guid1.CompareTo(guid2);
            if (compare < 0)
            {
                Id1 = citId1;
                Id2 = citId2;
            }
            else
            {
                Id1 = citId2;
                Id2 = citId1;
            }
            return $"{Id1}&{Id2}";
        }


        //Long Important Method
        //Updates the relationships
        //finds relationships that don't need to be stored on the playercompany and moves them to the relationshipcache
        //looks for any missing relationships and fixes the gaps:
        //takes missing relationships from the relationship cache and creates new ones for those that dont exist
        public void UpdateRelationships(RelationshipCache relationshipcache)
        {
            List<string> advisorIds = new();
            int relationshipCount = 0;
            int oldrelationships = 0;
            int newrelationships = 0;
            foreach (Citizen advisor in Advisors.Values)
            {
                advisorIds.Add(advisor.id.ToString());
            }
            //iterates through each relationship in Social
            foreach (KeyValuePair<string, Relationship> kvp in Relationships)
            {
                string key = kvp.Key;
                string[] ids = key.Split("&");
                string id1 = ids[0];
                string id2 = ids[1];
                //check to see if the key contains ids from two current advisors
                //if it doesnt match 2 current advisors, it removes it and stores it in the relationshipcache
                if (advisorIds.Contains(id1) && advisorIds.Contains(id2))
                {
                    relationshipCount++;
                }
                else
                {
                    oldrelationships++;
                    relationshipcache.CacheRelationship(kvp.Value);
                    Relationships.Remove(kvp.Key);
                }
            }
            //Iterates through each pair of advisors in the advisor list
            foreach (string id in advisorIds)
            {
                foreach (string id2 in advisorIds)
                {
                    //skips looking for themselves
                    if (id != id2)
                    {
                        string relationshipId = CreateRelationshipId(id, id2);
                        //Checks to see if a relationship already exists in the company
                        if (!Relationships.ContainsKey(relationshipId))
                        {
                            //checks to see if the relationship exists in the cache
                            if (relationshipcache.ContainsRelationships(relationshipId))
                            {
                                //retrieves the relationship from the cache if it exists there
                                Relationships.Add(relationshipId, relationshipcache.RetrieveRelationship(relationshipId));
                            }
                            //creates a new relationship if it doesnt exist anywhere
                            else
                            {
                                //turns the Ids back into citizens, uses lambda functions to search the advisors dictionary by a property instead of a key
                                //creates and then adds the new relationship to the playercompany
                                Citizen citizen1 = Advisors.Where(x => x.Value.id.ToString() == id).FirstOrDefault().Value;
                                Citizen citizen2 = Advisors.Where(x => x.Value.id.ToString() == id2).FirstOrDefault().Value;
                                Relationship newrelationship = new(citizen1, citizen2);
                                Relationships.Add(relationshipId, newrelationship);
                            }
                            newrelationships++;
                        }
                    }
                }
            }
            //TODO remove this tracking
            Console.WriteLine($"There are {relationshipCount} good relationships, and {oldrelationships} old relationships removed, with {newrelationships} new relationships added.");
        }

        public string ViewRecruits(CitizenCache citizenCache)
        {
            if (!Recruits.Any() || (LastRecruitRecycle + TimeSpan.FromDays(2)) < DateTime.Now)
            {
                //Replace the list
            }
            //display the info about them

            return "fix";
        }


        #endregion
    }
}
