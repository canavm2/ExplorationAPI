//using Newtonsoft.Json;
using People;
using Company;
using Relation;
using Users;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace FileTools
{
    //An object that gets instatiated, it holds the filepath to the folder everything is saved in.
    //It also holds all the methods used to read/write to the .txt files
    public class FileTool
    {
        #region Constructor and Lists
        public FileTool(string azureUri, string azureKey)
        {
            options.WriteIndented = true;
            cosmosClient = new CosmosClient(azureUri, azureKey);
            container = cosmosClient.GetDatabase(databaseId).GetContainer(containerId);
        }
        #endregion

        #region Dictionaries and Properties
        JsonSerializerOptions options = new JsonSerializerOptions();
        string databaseId = "ExplorationDB";
        CosmosClient cosmosClient;
        string containerId = "Caches";
        CosmosContainer container;
        #endregion

        #region methods
        public async Task StoreCitizens(CitizenCache citizens)
        {
            ItemResponse<CitizenCache> response = await container.UpsertItemAsync<CitizenCache>(citizens);
        }
        public async Task<CitizenCache> ReadCitizens(Guid id)
        {
            ItemResponse<CitizenCache> response = await container.ReadItemAsync<CitizenCache>(id: id.ToString(), partitionKey: new PartitionKey(id.ToString()));
            return (CitizenCache)response;
        }
        public async Task StoreCompanies(CompanyCache playerCompany)
        {
            ItemResponse<CompanyCache> response = await container.UpsertItemAsync<CompanyCache>(playerCompany);
        }
        public async Task<CompanyCache> ReadCompanies(Guid id)
        {
            ItemResponse<CompanyCache> response = await container.ReadItemAsync<CompanyCache>(id: id.ToString(), partitionKey: new PartitionKey(id.ToString()));
            return (CompanyCache)response;
        }
        public async Task StoreRelationshipCache(RelationshipCache relationships)
        {
            ItemResponse<RelationshipCache> response = await container.UpsertItemAsync<RelationshipCache>(relationships);
        }
        public async Task<RelationshipCache> ReadRelationshipCache(Guid id)
        {
            ItemResponse<RelationshipCache> response = await container.ReadItemAsync<RelationshipCache>(id: id.ToString(), partitionKey: new PartitionKey(id.ToString()));
            return (RelationshipCache)response;
        }
        public async Task StoreLoadTool(LoadTool loadTool)
        {
            ItemResponse<LoadTool> response = await container.UpsertItemAsync<LoadTool>(loadTool);
        }
        public async Task<LoadTool> ReadLoadTool(Guid id)
        {
            ItemResponse<LoadTool> response = await container.ReadItemAsync<LoadTool>(id: id.ToString(), partitionKey: new PartitionKey(id.ToString()));
            return (LoadTool)response;
        }
        public async Task StoreUsers(UserCache userCache)
        {
            ItemResponse<UserCache> response = await container.UpsertItemAsync<UserCache>(userCache);
        }
        public async Task<UserCache> ReadUsers(Guid id)
        {
            ItemResponse<UserCache> response = await container.ReadItemAsync<UserCache>(id: id.ToString(), partitionKey: new PartitionKey(id.ToString()));
            return (UserCache)response;
        }

        //public void StoreModifierList(ModifierList modifierlist, string filename)
        //{
        //    filename += ".txt";
        //    string jsonmodifierlist = JsonSerializer.Serialize(modifierlist, options);
        //    string filepath = Path.Combine(TxtFilePath, filename);
        //    File.WriteAllText(filepath, jsonmodifierlist);
        //}
        //public ModifierList ReadModifierList(string filename)
        //{
        //    filename += ".txt";
        //    string filepath = Path.Combine(TxtFilePath, filename);
        //    string fileJson = File.ReadAllText(filepath);
        //    ModifierList modifierlist = JsonSerializer.Deserialize<ModifierList>(fileJson);
        //    return modifierlist;
        //}
        //public void StoreTraitList(TraitList traitlist, string filename)
        //{
        //    filename += ".txt";
        //    string jsontraitlist = JsonSerializer.Serialize(traitlist, options);
        //    string filepath = Path.Combine(TxtFilePath, filename);
        //    File.WriteAllText(filepath, jsontraitlist);
        //}
        //public TraitList ReadTraitList(string filename)
        //{
        //    filename += ".txt";
        //    string filepath = Path.Combine(TxtFilePath, filename);
        //    string fileJson = File.ReadAllText(filepath);
        //    TraitList traitlist = JsonSerializer.Deserialize<TraitList>(fileJson);
        //    return traitlist;
        //}
        //public void StoreModifier(Modifier modifier)
        //{
        //    string filepath = Path.Combine(TxtFilePath, "modifier.txt");
        //    string jsoncitizen = JsonSerializer.Serialize(modifier, options);
        //    File.WriteAllText(filepath, jsoncitizen);
        //}
        //public Modifier ReadModifier()
        //{
        //    string filepath = Path.Combine(TxtFilePath, "modifier.txt");
        //    string fileJson = File.ReadAllText(filepath);
        //    Modifier modifier = JsonSerializer.Deserialize<Modifier>(fileJson);
        //    return modifier;
        //}
        #endregion
    }
    //An object that can be isntatiated to hold the lists of skills, stats, and attributes
    public class ListTool
    {
        public ListTool(){}
        public List<string> VocSkillsList = new()
        {
            "Academia",
            "Athletics",
            "Animal Handling",
            "Blacksmithing",
            "Carpentry",
            "Cooking",
            "Diplomacy",
            "Drill",
            "Engineering",
            "First Aid",
            "History",
            "Hunting",
            "Law",
            "Leadership",
            "Leatherworking",
            "Martial",
            "Medical",
            "Metalworking",
            "Pathfinding",
            "Persuation",
            "Politics",
            "Prospecting",
            "Refining",
            "Quartermastery",
            "Skullduggery",
            "Stealth",
            "Survival",
            "Tactics",
            "Tinker"
        };

        public List<string> ExpSkillsList = new() { "exp1", "exp2", "exp3" };
        public List<string> PrimaryStats = new() { "STR", "DEX", "INT", "WIS", "CHA", "LDR" };
        public List<string> DerivedStats = new() { "PHYS", "MNTL", "SOCL" };
        public List<string> Attributes = new List<string>() {
                "Health",
                "Happiness",
                "Motivation",
                "Psyche"
            };
    }
    public class LoadTool
    {
        #region Constructors
        public LoadTool()
        {
            id = Guid.NewGuid();
        }

        [JsonConstructor]
        public LoadTool(Guid Id, Guid citizenCacheId, Guid relationshipCacheId, Guid companyCacheId, Guid userCacheId)
        {
            id = Id;
            CitizenCacheId = citizenCacheId;
            RelationshipCacheId = relationshipCacheId;
            CompanyCacheId = companyCacheId;
            UserCacheId = userCacheId;
        }
        #endregion

        #region Dictionaries and Properties
        public Guid id { get; set; }
        public Guid CitizenCacheId { get; set; }
        public Guid RelationshipCacheId { get; set; }
        public Guid CompanyCacheId { get; set; }
        public Guid UserCacheId { get; set; }
        #endregion
    }
}
