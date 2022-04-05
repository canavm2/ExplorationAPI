using Azure.Identity;
using Company;
using FileTools;
using Microsoft.OpenApi.Models;
using People;
using Relation;
using Users;
using APIMethods;
using DependencyInjection;
using Azure.Security.KeyVault.Secrets;

#region BuildFileTool
var client = new SecretClient(new Uri("https://explorationkv.vault.azure.net/"), new DefaultAzureCredential());
var azureUriSecret = await client.GetSecretAsync("AzureUri");
var azureKeySecret = await client.GetSecretAsync("PrimaryKey");
string azureUri = azureUriSecret.Value.Value;
string azureKey = azureKeySecret.Value.Value;
FileTool fileTool = new FileTool(azureUri, azureKey);
LoadTool loadTool = await fileTool.ReadLoadTool(new Guid("fd46a92d-5c61-4afa-b6bf-63876fae3a5c"));
#endregion

Boolean NewData = false;

#region CitizenLoading
CitizenCache citizenCache;
if (NewData)
{
    citizenCache = new CitizenCache(100);
    Console.WriteLine($"femalecitizens has: {citizenCache.FemaleCitizens.Count} items.\nThe first female is:\n{citizenCache.FemaleCitizens[0].Describe()}");
    Console.WriteLine($"malecitizens has: {citizenCache.MaleCitizens.Count} items.\nThe first male is:\n{citizenCache.MaleCitizens[0].Describe()}");
    Console.WriteLine($"nbcitizens has: {citizenCache.NBCitizens.Count} items.\nThe first non-binary is:\n{citizenCache.NBCitizens[0].Describe()}");
    loadTool.CitizenCacheId = citizenCache.id;
}
else citizenCache = await fileTool.ReadCitizens(loadTool.CitizenCacheId);
#endregion

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<ICitizenCache>(citizenCache);
//builder.Services.RegisterAuth();
//builder.Services.RegisterRepos();
//builder.Services.RegisterLogging();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new OpenApiInfo()
    {
        Description = "This is effectively the client for now.",
        Title = "Exploration Game",
        Version = "v1"
    });
});

var app = builder.Build();
//builder.Configuration.AddAzureKeyVault(new Uri("https://explorationkv.vault.azure.net/"), new DefaultAzureCredential());
//azureUri = builder.Configuration["AzureUri"];
//azureKey = builder.Configuration["PrimaryKey"];

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

#region dataloading
//FileTool fileTool = new FileTool(azureUri, azureKey);
//LoadTool loadTool = await fileTool.ReadLoadTool(new Guid("fd46a92d-5c61-4afa-b6bf-63876fae3a5c"));
//LoadTool loadTool = new();
#endregion

//Boolean NewData = false;

#region Citizen Loading
//ICitizenCache citizenCache;
//if (NewData)
//{
//    citizenCache = new CitizenCache(100);
//    Console.WriteLine($"femalecitizens has: {citizenCache.FemaleCitizens.Count} items.\nThe first female is:\n{citizenCache.FemaleCitizens[0].Describe()}");
//    Console.WriteLine($"malecitizens has: {citizenCache.MaleCitizens.Count} items.\nThe first male is:\n{citizenCache.MaleCitizens[0].Describe()}");
//    Console.WriteLine($"nbcitizens has: {citizenCache.NBCitizens.Count} items.\nThe first non-binary is:\n{citizenCache.NBCitizens[0].Describe()}");
//    loadTool.CitizenCacheId = citizenCache.id;
//}
//else citizenCache = await fileTool.ReadCitizens(loadTool.CitizenCacheId);
#endregion

#region User Loading
UserCache userCache;
if (NewData)
{
    userCache = new();
    loadTool.UserCacheId = userCache.id;
}
else userCache = await fileTool.ReadUsers(loadTool.UserCacheId);
#endregion

#region Company Loading
CompanyCache companyCache;
if (NewData)
{
    companyCache = new();
    loadTool.CompanyCacheId = companyCache.id;
}
else companyCache = await fileTool.ReadCompanies(loadTool.CompanyCacheId);
#endregion

#region Relationship Loading
RelationshipCache relationshipCache;
if (NewData)
{
    relationshipCache = new RelationshipCache();
    loadTool.RelationshipCacheId = relationshipCache.id;
}
else relationshipCache = await fileTool.ReadRelationshipCache(loadTool.RelationshipCacheId);
#endregion

#region Save Data
await fileTool.StoreLoadTool(loadTool);
if (NewData)
{
    await fileTool.StoreCitizens((CitizenCache)citizenCache);
    await fileTool.StoreCompanies(companyCache);
    await fileTool.StoreRelationshipCache(relationshipCache);
    await fileTool.StoreUsers(userCache);
}
#endregion

app.MapGet("/advancesave", () => APICalls.AdvanceSave(fileTool, citizenCache, userCache, companyCache, relationshipCache));
app.MapGet("/createuser/{username}", (string username) => APICalls.CreateUser(username, userCache, citizenCache, companyCache));
app.MapGet("/company/{username}", (string username) => APICalls.StandardInfo(username, userCache, companyCache));
app.MapGet("/company/{username}/advisor/{role}", (string username, string role) => companyCache.PlayerCompanies[userCache.Users[username].CompanyId].Advisors[role].Describe());
app.MapGet("/company/{username}/spendtp/{tp}", (string username, double tp) => APICalls.SpendTp(username, tp, userCache, companyCache));

app.Run();
