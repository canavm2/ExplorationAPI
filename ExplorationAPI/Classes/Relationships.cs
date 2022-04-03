using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using People;
using Company;
//using Newtonsoft.Json;

namespace Relation
{
    //Object that holds the old relationships
    //these are relationships between citizens that are no longer in the same company
    public class RelationshipCache
    {
        #region constructors
        public RelationshipCache()
        {
            OldRelationships = new();
            id = Guid.NewGuid();
        }

        [JsonConstructor]
        public RelationshipCache(Dictionary<string, Relationship> oldrelationships, Guid Id)
        {
            OldRelationships = oldrelationships;
            id = Id;
        }
        #endregion

        #region dictionaries and attributes
        public Guid id { get; set; }
        public Dictionary<string, Relationship> OldRelationships { get; set; }
        #endregion

        #region methods
        public void CacheRelationship(Relationship relationship)
        {
            if (!OldRelationships.ContainsKey(relationship.id))
                OldRelationships[relationship.id] = relationship;
            else throw new Exception($"Relationship Cache already contains relationship: {relationship.id}");
        }
        public Relationship RetrieveRelationship(string id)
        {
            Relationship retriviedRelationship = OldRelationships[id];
            OldRelationships.Remove(id);
            return retriviedRelationship;
        }
        public bool ContainsRelationships(string id)
        {
            return OldRelationships.ContainsKey(id);
        }
        #endregion
    }

    //Object that holds the various relationships between 2 citizens, only 1 object should exist, not one for each citizen
    public class Relationship
    {
        #region Constructors
        public Relationship(Citizen citizen1, Citizen citizen2)
        {
            Random random = new Random();
            Friendliness = random.Next(-10, 20);
            Teamwork = random.Next(-10, 20);
            Connection = 0;
            //Creates an ID with the citizen's IDs as: XXXX-YYYY, where XXXX is smaller than YYYY
            int compare = citizen1.id.CompareTo(citizen2.id);
            id = CreateRelationshipId(citizen1.id.ToString(), citizen2.id.ToString(), compare);
            if (compare < 0)
            {
                Citizen1Name = citizen1.Name;
                Citizen2Name = citizen2.Name;
            }
            else
            {
                Citizen1Name = citizen2.Name;
                Citizen2Name = citizen1.Name;
            }
        }

        [JsonConstructor]
        public Relationship(int friendliness, int teamwork, int connection, string Id, string citizen1name, string citizen2name)
        {
            Friendliness = friendliness;
            Teamwork = teamwork;
            Connection = connection;
            id = Id;
            Citizen1Name = citizen1name;
            Citizen2Name = citizen2name;
        }
        #endregion

        #region Dictionaries and Properties
        //This is not a Guid because its stored as two Guids merged with an "&"
        public string id { get; set; }
        public string Citizen1Name { get; set; }
        public string Citizen2Name { get; set; }
        public int Friendliness { get; set; }
        public int Teamwork { get; set; }
        public int Connection { get; set; }
        #endregion

        #region Methods
        //Creates an ID with the citizen's IDs as: XXXX-YYYY, where XXXX is smaller than YYYY
        public static string CreateRelationshipId(string id1, string id2, int compare)
        {
            string Id1;
            string Id2;
            if (compare < 0)
            {
                Id1 = id1;
                Id2 = id2;
            }
            else
            {
                Id1 = id2;
                Id2 = id1;
            }
            return $"{Id1}&{Id2}";
        }
        #endregion
    }
}
