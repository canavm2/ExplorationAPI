﻿//using Newtonsoft.Json;
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
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;

namespace FileTools
{
    //An object that gets instatiated, it holds the filepath to the folder everything is saved in.
    //It also holds all the methods used to read/write to the .txt files
    public static class StartupTools
    {
        public static async Task<FileTool> ConstructFileTool()
        {
            var client = new SecretClient(new Uri("https://explorationkv.vault.azure.net/"), new DefaultAzureCredential());
            var azureUriSecret = await client.GetSecretAsync("AzureUri");
            var azureKeySecret = await client.GetSecretAsync("PrimaryKey");
            var LoginKey = await client.GetSecretAsync("LoginKey");
            var AdminCheck = await client.GetSecretAsync("AdminCheck");
            string azureUri = azureUriSecret.Value.Value;
            string azureKey = azureKeySecret.Value.Value;
            string loginKey = LoginKey.Value.Value;
            string adminCheck = AdminCheck.Value.Value;
            return new FileTool(azureUri, azureKey, loginKey, adminCheck);
        }
        public static async Task<CitizenCache> ConstructCitizenCache(Boolean newData, LoadTool loadTool, FileTool fileTool)
        {
            CitizenCache citizenCache;
            if (newData)
            {
                citizenCache = new CitizenCache(100);
                //Console.WriteLine($"femalecitizens has: {citizenCache.FemaleCitizens.Count} items.\nThe first female is:\n{citizenCache.FemaleCitizens[0].Describe()}");
                //Console.WriteLine($"malecitizens has: {citizenCache.MaleCitizens.Count} items.\nThe first male is:\n{citizenCache.MaleCitizens[0].Describe()}");
                //Console.WriteLine($"nbcitizens has: {citizenCache.NBCitizens.Count} items.\nThe first non-binary is:\n{citizenCache.NBCitizens[0].Describe()}");
                loadTool.CitizenCacheId = citizenCache.id;
            }
            else citizenCache = await fileTool.ReadCitizens(loadTool.CitizenCacheId);
            return citizenCache;
        }
        public static async Task<CompanyCache> ConstructCompanyCache(Boolean newData, LoadTool loadTool, FileTool fileTool)
        {
            CompanyCache companyCache;
            if (newData)
            {
                companyCache = new();
                loadTool.CompanyCacheId = companyCache.id;
            }
            else companyCache = await fileTool.ReadCompanies(loadTool.CompanyCacheId);
            return companyCache;
        }
        public static async Task<UserCache> ConstructUserCache(Boolean newData, LoadTool loadTool, FileTool fileTool)
        {
            UserCache userCache;
            if (newData)
            {
                userCache = new();
                loadTool.UserCacheId = userCache.id;
            }
            else userCache = await fileTool.ReadUsers(loadTool.UserCacheId);
            return userCache;
        }
        public static async Task<RelationshipCache> ConstructRelationshipCache(Boolean newData, LoadTool loadTool, FileTool fileTool)
        {
            RelationshipCache relationshipCache;
            if (newData)
            {
                relationshipCache = new RelationshipCache();
                loadTool.RelationshipCacheId = relationshipCache.id;
            }
            else relationshipCache = await fileTool.ReadRelationshipCache(loadTool.RelationshipCacheId);
            return relationshipCache;
        }
    }


    public interface IFileTool
    {
        JsonSerializerOptions options { get; set; }
        string databaseId { get; set; }
        CosmosClient cosmosClient { get; set; }
        string containerId { get; set; }
        CosmosContainer container { get; set; }
        string LoginKey { get; set; }
        string AdminCheck { get; set; }
        public Task StoreCitizens(CitizenCache citizens);
        public Task<CitizenCache> ReadCitizens(Guid id);
        public Task StoreCompanies(CompanyCache playerCompany);
        public Task<CompanyCache> ReadCompanies(Guid id);
        public Task StoreRelationshipCache(RelationshipCache relationships);
        public Task<RelationshipCache> ReadRelationshipCache(Guid id);
        public Task StoreLoadTool(LoadTool loadTool);
        public Task<LoadTool> ReadLoadTool(Guid id);
        public Task StoreUsers(UserCache userCache);
        public Task<UserCache> ReadUsers(Guid id);
    }
    public class FileTool : IFileTool
    {
        #region Constructor and Lists
        public FileTool(string azureUri, string azureKey, string loginKey, string adminCheck)
        {
            databaseId = "ExplorationDB";
            containerId = "Caches";
            options = new JsonSerializerOptions();
            options.WriteIndented = true;
            cosmosClient = new CosmosClient(azureUri, azureKey);
            container = cosmosClient.GetDatabase(databaseId).GetContainer(containerId);
            LoginKey = loginKey;
            AdminCheck = adminCheck;
        }
        #endregion

        #region Dictionaries and Properties
        public JsonSerializerOptions options { get; set; }
        public string databaseId { get; set; }
        public CosmosClient cosmosClient { get; set; }
        public string containerId { get; set; }
        public CosmosContainer container { get; set; }
        public string LoginKey { get; set; }
        public string AdminCheck { get; set; }
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
        public Dictionary<string, List<string>> SkillsList = new()
        {
            // TODO FIX THESE, Stats are wrong
            {"Academia", new List<string>{"INT","WIS"} },
            {"Athletics", new List<string>{"STR","AGI"} },
            {"Animal Handling", new List<string>{"CHA","WIS"} },
            {"Blacksmithing", new List<string>{"STR","INT"} },
            {"Carpentry", new List<string>{"INT","AGI"} },
            {"Cooking", new List<string>{"WIS","CHA"} },
            {"Diplomacy", new List<string>{"LDR","CHA"} },
            {"Drill", new List<string>{"LDR","AGI"} },
            {"Engineering", new List<string>{"INT","WIS"} },
            {"First Aid", new List<string>{"WIS","CHA"} },
            {"History", new List<string>{"WIS","INT"} },
            {"Hunting", new List<string>{"AGI","WIS"} },
            {"Law", new List<string>{"INT","WIS"} },
            {"Leadership", new List<string>{"LDR","WIS"} },
            {"Leatherworking", new List<string>{"WIS","AGI"} },
            {"Martial", new List<string>{"INT","AGI"} },
            {"Medical", new List<string>{"INT","AGI"} },
            {"Metalworking", new List<string>{"STR","INT"} },
            {"Pathfinding", new List<string>{"WIS","INT"} },
            {"Persuation", new List<string>{"CHA","WIS"} },
            {"Politics", new List<string>{"LDR","CHA"} },
            {"Prospecting", new List<string>{"INT","WIS"} },
            {"Refining", new List<string>{"INT","WIS"} },
            {"Quartermastery", new List<string>{"INT","WIS"} },
            {"Skullduggery", new List<string>{"INT","WIS"} },
            {"Stealth", new List<string>{"INT","WIS"} },
            {"Survival", new List<string>{"INT","WIS"} },
            {"Tactics", new List<string>{"INT","WIS"} },
            {"Tinker", new List<string>{"INT","WIS"} }
        };

        public List<string> ExpSkillsList = new() { "exp1", "exp2", "exp3" };
        public List<string> PrimaryStats = new() { "STR", "AGI", "CON", "INT", "WIS", "PER", "CHA", "LDR", "WIL"};
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
