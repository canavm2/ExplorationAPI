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
    public partial class PlayerCompany
    {
        #region Methods
        public string Describe()
        {
            string advisorDescription = "";
            foreach (var advisor in Advisors)
            {
                advisorDescription += $"{advisor.Name}\n";
            }
            string benchDescription = "";
            foreach (var bench in Bench)
            {
                benchDescription += $"{bench.Name}\n";
            }
            string companyDescription =
                $"The company's name is: {Name}.\n" +
                $"ID: {id}\n\n" +
                $"The company master is FIX MASTER NAME HERE.\n\n" +
                $"The company advisors are:\n" +
                advisorDescription +
                $"\nThese advisors are on the bench:\n" +
                benchDescription +
                $"\nThe company skills are:\n" +
                DescribeCompanySkills()
                ;
            return companyDescription;
        }

        public string DescribeCompanySkills()
        {
            string primaryDesc = "";
            foreach (var skill in Skills)
            {
                primaryDesc += "{skill.Key.ToUpper()}: {skill.Value.Full}\n";
            }
            return primaryDesc;
        }

        //Ensure all the skills are up to date
        internal void UpdateCompanySkills()
        {
            foreach (var skill in Skills)
            {
                UpdateCompanySkill(skill.Key);
            }
        }
        //Update a single skill
        internal void UpdateCompanySkill(Skill skill)
        {
            List<int> skillvalues = new();
            foreach (Citizen citizen in Advisors)
            {
                skillvalues.Add(citizen.GetSkill(skill));
            }
            skillvalues.Sort();
            skillvalues.Reverse();
            Skills[skill] = new((skillvalues[0] + skillvalues[1]) / 2);
        }

        //Add an advisor to an EMPTY role, rarely used, except initial construction and vacant roles
        internal void AddMember(Citizen citizen, string role)
        {
            //TODO verify the role is acceptible
            _members[role] = citizen;
            
            // TODO Update relationships!!!
        }

        ////Add an advisor to an occupied role, most common, saves the old citizen to the citizen cache
        //public void ReplaceAdvisor(Citizen citizen, string role, CitizenCache citizencache, RelationshipCache relationshipcache)
        //{
        //    if (!Advisors.ContainsKey(role)) throw new Exception($"No citizen to replace in role: {role}.");
        //    Citizen replacedCitizen = Advisors[role];
        //    Advisors[role] = citizen;
        //    citizencache.CacheCitizen(replacedCitizen);
        //    UpdateCompanySkills();
        //}

        //public string CreateRelationshipId(string citId1, string citId2)
        //{
        //    string Id1;
        //    string Id2;
        //    Guid guid1 = new Guid(citId1);
        //    Guid guid2 = new Guid(citId2);
        //    int compare = guid1.CompareTo(guid2);
        //    if (compare < 0)
        //    {
        //        Id1 = citId1;
        //        Id2 = citId2;
        //    }
        //    else
        //    {
        //        Id1 = citId2;
        //        Id2 = citId1;
        //    }
        //    return $"{Id1}&{Id2}";
        //}


        //Long Important Method
        //Updates the relationships
        //finds relationships that don't need to be stored on the playercompany and moves them to the relationshipcache
        //looks for any missing relationships and fixes the gaps:
        //takes missing relationships from the relationship cache and creates new ones for those that dont exist
        //public void UpdateRelationships(RelationshipCache relationshipcache)
        //{
        //    List<string> advisorIds = new();
        //    int relationshipCount = 0;
        //    int oldrelationships = 0;
        //    int newrelationships = 0;
        //    foreach (Citizen advisor in Advisors.Values)
        //    {
        //        advisorIds.Add(advisor.id.ToString());
        //    }
        //    //iterates through each relationship in Social
        //    foreach (KeyValuePair<string, Relationship> kvp in Relationships)
        //    {
        //        string key = kvp.Key;
        //        string[] ids = key.Split("&");
        //        string id1 = ids[0];
        //        string id2 = ids[1];
        //        //check to see if the key contains ids from two current advisors
        //        //if it doesnt match 2 current advisors, it removes it and stores it in the relationshipcache
        //        if (advisorIds.Contains(id1) && advisorIds.Contains(id2))
        //        {
        //            relationshipCount++;
        //        }
        //        else
        //        {
        //            oldrelationships++;
        //            relationshipcache.CacheRelationship(kvp.Value);
        //            Relationships.Remove(kvp.Key);
        //        }
        //    }
        //    //Iterates through each pair of advisors in the advisor list
        //    foreach (string id in advisorIds)
        //    {
        //        foreach (string id2 in advisorIds)
        //        {
        //            //skips looking for themselves
        //            if (id != id2)
        //            {
        //                string relationshipId = CreateRelationshipId(id, id2);
        //                //Checks to see if a relationship already exists in the company
        //                if (!Relationships.ContainsKey(relationshipId))
        //                {
        //                    //checks to see if the relationship exists in the cache
        //                    if (relationshipcache.ContainsRelationships(relationshipId))
        //                    {
        //                        //retrieves the relationship from the cache if it exists there
        //                        Relationships.Add(relationshipId, relationshipcache.RetrieveRelationship(relationshipId));
        //                    }
        //                    //creates a new relationship if it doesnt exist anywhere
        //                    else
        //                    {
        //                        //turns the Ids back into citizens, uses lambda functions to search the advisors dictionary by a property instead of a key
        //                        //creates and then adds the new relationship to the playercompany
        //                        Citizen citizen1 = Advisors.Where(x => x.Value.id.ToString() == id).FirstOrDefault().Value;
        //                        Citizen citizen2 = Advisors.Where(x => x.Value.id.ToString() == id2).FirstOrDefault().Value;
        //                        Relationship newrelationship = new(citizen1, citizen2);
        //                        Relationships.Add(relationshipId, newrelationship);
        //                    }
        //                    newrelationships++;
        //                }
        //            }
        //        }
        //    }
        //    //TODO remove this tracking
        //    Console.WriteLine($"There are {relationshipCount} good relationships, and {oldrelationships} old relationships removed, with {newrelationships} new relationships added.");
        //}

        //public string ViewRecruits(CitizenCache citizenCache)
        //{
        //    if (!Recruits.Any() || (LastRecruitRecycle + TimeSpan.FromDays(2)) < DateTime.Now)
        //    {
        //        //Replace the list
        //    }
        //    //display the info about them

        //    return "fix";
        //}
        #endregion
    }
}
