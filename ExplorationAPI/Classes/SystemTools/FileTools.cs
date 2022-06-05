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
        //public static async Task<RelationshipCache> ConstructRelationshipCache(Boolean newData, LoadTool loadTool, FileTool fileTool)
        //{
        //    RelationshipCache relationshipCache;
        //    if (newData)
        //    {
        //        relationshipCache = new RelationshipCache();
        //        loadTool.RelationshipCacheId = relationshipCache.id;
        //    }
        //    else relationshipCache = await fileTool.ReadRelationshipCache(loadTool.RelationshipCacheId);
        //    return relationshipCache;
        //}
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
        //public Task StoreRelationshipCache(RelationshipCache relationships);
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

        #region Methods
        public async Task StoreTest(CitizenCache citizens)
        {
            containerId = "testCache";
            container = cosmosClient.GetDatabase(databaseId).GetContainer(containerId);
            ItemResponse<CitizenCache> response = await container.UpsertItemAsync<CitizenCache>(citizens);
        }
        public async Task StoreCitizens(CitizenCache citizens)
        {
            containerId = "CitizenCache";
            container = cosmosClient.GetDatabase(databaseId).GetContainer(containerId);
            ItemResponse<CitizenCache> response = await container.UpsertItemAsync<CitizenCache>(citizens);
        }
        public async Task<CitizenCache> ReadCitizens(Guid id)
        {
            containerId = "CitizenCache";
            container = cosmosClient.GetDatabase(databaseId).GetContainer(containerId);
            ItemResponse<CitizenCache> response = await container.ReadItemAsync<CitizenCache>(id: id.ToString(), partitionKey: new PartitionKey(id.ToString()));
            return (CitizenCache)response;
        }
        public async Task StoreCompanies(CompanyCache playerCompany)
        {
            containerId = "CompanyCache";
            container = cosmosClient.GetDatabase(databaseId).GetContainer(containerId);
            ItemResponse<CompanyCache> response = await container.UpsertItemAsync<CompanyCache>(playerCompany);
        }
        public async Task<CompanyCache> ReadCompanies(Guid id)
        {
            containerId = "CompanyCache";
            container = cosmosClient.GetDatabase(databaseId).GetContainer(containerId);
            ItemResponse<CompanyCache> response = await container.ReadItemAsync<CompanyCache>(id: id.ToString(), partitionKey: new PartitionKey(id.ToString()));
            return (CompanyCache)response;
        }
        public async Task StoreRelationshipCache(RelationshipCache relationships)
        {
            containerId = "RelationshipCache";
            container = cosmosClient.GetDatabase(databaseId).GetContainer(containerId);
            ItemResponse<RelationshipCache> response = await container.UpsertItemAsync<RelationshipCache>(relationships);
        }
        public async Task<RelationshipCache> ReadRelationshipCache(Guid id)
        {
            containerId = "RelationshipCache";
            container = cosmosClient.GetDatabase(databaseId).GetContainer(containerId);
            ItemResponse<RelationshipCache> response = await container.ReadItemAsync<RelationshipCache>(id: id.ToString(), partitionKey: new PartitionKey(id.ToString()));
            return (RelationshipCache)response;
        }
        public async Task StoreLoadTool(LoadTool loadTool)
        {
            containerId = "LoadTool";
            container = cosmosClient.GetDatabase(databaseId).GetContainer(containerId);
            ItemResponse<LoadTool> response = await container.UpsertItemAsync<LoadTool>(loadTool);
        }
        public async Task<LoadTool> ReadLoadTool(Guid id)
        {
            containerId = "LoadTool";
            container = cosmosClient.GetDatabase(databaseId).GetContainer(containerId);
            ItemResponse<LoadTool> response = await container.ReadItemAsync<LoadTool>(id: id.ToString(), partitionKey: new PartitionKey(id.ToString()));
            return (LoadTool)response;
        }
        public async Task StoreUsers(UserCache userCache)
        {
            containerId = "UserCache";
            container = cosmosClient.GetDatabase(databaseId).GetContainer(containerId);
            ItemResponse<UserCache> response = await container.UpsertItemAsync<UserCache>(userCache);
        }
        public async Task<UserCache> ReadUsers(Guid id)
        {
            containerId = "UserCache";
            container = cosmosClient.GetDatabase(databaseId).GetContainer(containerId);
            ItemResponse<UserCache> response = await container.ReadItemAsync<UserCache>(id: id.ToString(), partitionKey: new PartitionKey(id.ToString()));
            return (UserCache)response;
        }

        public async Task<bool> ResetContainers()
        {
            Dictionary<string, string> containers = new ();
            containers.Add("CitizenCache", "/id");
            containers.Add("CompanyCache", "/id");
            containers.Add("UserCache", "/id");

            foreach (var c in containers)
            {
                try
                {
                    var container = cosmosClient.GetDatabase(databaseId).GetContainer(c.Key);

                    await container.DeleteContainerAsync();
                }
                catch
                {
                }


                await cosmosClient.GetDatabase(databaseId).CreateContainerIfNotExistsAsync(c.Key, c.Value);
            }
            return true;
        }

        #endregion
    }
    //An object that can be isntatiated to hold the lists of skills, stats, and attributes
    public class ListTool
    {
        public ListTool(){}
        public Dictionary<Skill, List<Stat>> SkillsList = new()
        {
            // TODO FIX THESE, Stats are wrong
            {Skill.Academia, new List<Stat>{Stat.INT, Stat.WIS } },
            {Skill.Athletics, new List<Stat>{ Stat.STR, Stat.AGI } },
            {Skill.AnimalHandling, new List<Stat>{ Stat.CHA, Stat.WIS } },
            {Skill.Blacksmithing, new List<Stat>{ Stat.STR, Stat.INT } },
            {Skill.Carpentry, new List<Stat>{ Stat.INT, Stat.AGI } },
            {Skill.Cooking, new List<Stat>{ Stat.WIS, Stat.CHA } },
            {Skill.Diplomacy, new List<Stat>{ Stat.LDR, Stat.CHA } },
            {Skill.Drill, new List<Stat>{ Stat.LDR, Stat.AGI } },
            {Skill.Engineering, new List<Stat>{ Stat.INT, Stat.WIS } },
            {Skill.FirstAid, new List<Stat>{ Stat.WIS, Stat.CHA } },
            {Skill.History, new List<Stat>{ Stat.WIS, Stat.INT } },
            {Skill.Hunting, new List<Stat>{ Stat.AGI, Stat.WIS } },
            {Skill.Law, new List<Stat>{ Stat.INT, Stat.WIS } },
            {Skill.Leadership, new List<Stat>{ Stat.LDR, Stat.WIS } },
            {Skill.Leatherworking, new List<Stat>{ Stat.WIS, Stat.AGI } },
            {Skill.Martial, new List<Stat>{ Stat.INT, Stat.AGI } },
            {Skill.Medical, new List<Stat>{ Stat.INT, Stat.AGI } },
            {Skill.Metalworking, new List<Stat>{ Stat.STR, Stat.INT } },
            {Skill.Pathfinding, new List<Stat>{ Stat.WIS, Stat.INT } },
            {Skill.Persuation, new List<Stat>{ Stat.CHA, Stat.WIS } },
            {Skill.Politics, new List<Stat>{ Stat.LDR, Stat.CHA } },
            {Skill.Prospecting, new List<Stat>{ Stat.INT, Stat.WIS } },
            {Skill.Refining, new List<Stat>{ Stat.INT, Stat.WIS } },
            {Skill.Quartermastery, new List<Stat>{ Stat.INT, Stat.WIS } },
            {Skill.Skullduggery, new List<Stat>{ Stat.INT, Stat.WIS } },
            {Skill.Stealth, new List<Stat>{ Stat.INT, Stat.WIS } },
            {Skill.Survival, new List<Stat>{ Stat.INT, Stat.WIS } },
            {Skill.Tactics, new List<Stat>{ Stat.INT, Stat.WIS } },
            {Skill.Tinker, new List<Stat>{ Stat.INT, Stat.WIS } }
        };

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
            // TODO get rid of relationshipcaheid?
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
