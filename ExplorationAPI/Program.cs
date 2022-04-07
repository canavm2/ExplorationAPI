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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Swashbuckle.AspNetCore.Filters;

Boolean NewData = false;
Guid LoadToolGuid = new Guid("fd46a92d-5c61-4afa-b6bf-63876fae3a5c");


FileTool fileTool = await StartupTools.ConstructFileTool();
LoadTool loadTool = await fileTool.ReadLoadTool(LoadToolGuid);
CitizenCache citizenCache = await StartupTools.ConstructCitizenCache(NewData, loadTool, fileTool);
CompanyCache companyCache = await StartupTools.ConstructCompanyCache(NewData, loadTool, fileTool);
UserCache userCache = await StartupTools.ConstructUserCache(NewData, loadTool, fileTool);
RelationshipCache relationshipCache = await StartupTools.ConstructRelationshipCache(NewData, loadTool, fileTool);


var builder = WebApplication.CreateBuilder(args);

var keyVaultEndpoint = new Uri("https://explorationkv.vault.azure.net/");
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

builder.Services.AddControllers();
builder.Services.RegisterAuth();
builder.Services.RegisterRepos(citizenCache,companyCache,userCache,relationshipCache,fileTool);
builder.Services.RegisterLogging();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Description = "This is effectively the client for now.",
        Title = "Exploration Game",
        Version = "v1"
    });
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "This will be the standard authorization header using the bearer scheme (\"bearer {token}\"). \n" +
        "In order to authenticate, in the box below type \"bearer {token}\" and replace {token} with the long string that is returned from the login call.",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(fileTool.LoginKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

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
//app.MapGet("/createuser/{username}", (string username) => APICalls.CreateUser(username, userCache, citizenCache, companyCache));
app.MapGet("/company/{username}", (string username) => APICalls.StandardInfo(username, userCache, companyCache));
app.MapGet("/company/{username}/advisor/{role}", (string username, string role) => companyCache.PlayerCompanies[userCache.Users[username].CompanyId].Advisors[role].Describe());
app.MapGet("/company/{username}/spendtp/{tp}", (string username, double tp) => APICalls.SpendTp(username, tp, userCache, companyCache));

app.Run();
