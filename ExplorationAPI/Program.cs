global using Company;
global using People;
//global using Relation;
global using Users;
global using FileTools;
global using Events;
global using APIMethods;
using Azure.Identity;
using Microsoft.OpenApi.Models;
using APIMethods;
using DependencyInjection;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Swashbuckle.AspNetCore.Filters;

Boolean NewData = false;
Boolean NewLoad = false;

FileTool fileTool;
LoadTool loadTool;

if (NewLoad){
    fileTool = await StartupTools.ConstructFileTool();
    loadTool = new();
    await fileTool.StoreLoadTool(loadTool);
}
else
{
    Guid LoadToolGuid = new Guid("6462196d-7001-4304-8655-4f47c09630d6");
    fileTool = await StartupTools.ConstructFileTool();
    loadTool = await fileTool.ReadLoadTool(LoadToolGuid);
}

#region DataSetup
CitizenCache citizenCache = await StartupTools.ConstructCitizenCache(NewData, loadTool, fileTool);
CompanyCache companyCache = await StartupTools.ConstructCompanyCache(NewData, loadTool, fileTool);
UserCache userCache = await StartupTools.ConstructUserCache(NewData, loadTool, fileTool);
var keyVaultEndpoint = new Uri("https://explorationkv.vault.azure.net/");
await fileTool.StoreLoadTool(loadTool);
if (NewData)
{
    await fileTool.ResetContainers();
    await fileTool.StoreCompanies(companyCache);
    await fileTool.StoreCitizens(citizenCache);
    await fileTool.StoreUsers(userCache);
}
#endregion

var builder = WebApplication.CreateBuilder(args);

#region ConfigureBuilder
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());
builder.Services.AddControllers();
builder.Services.AddControllers();
//Next Three lines pull from DependencyInjection.cs
builder.Services.RegisterAuth();
builder.Services.RegisterRepos(citizenCache,companyCache,userCache,fileTool);
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
    //This creates the authenticate box in swagger
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
//Part of the Token setup https://www.youtube.com/watch?v=v7q3pEK1EA0
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
#endregion

var app = builder.Build();

#region AppBuilding
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
#endregion

#region MinimalAPI
//app.MapGet("/advancesave", () => APICalls.AdvanceSave(fileTool, citizenCache, userCache, companyCache, relationshipCache));
//app.MapGet("/createuser/{username}", (string username) => APICalls.CreateUser(username, userCache, citizenCache, companyCache));
//app.MapGet("/company/{username}", (string username) => APICalls.StandardInfo(username, userCache, companyCache));
//app.MapGet("/company/{username}/advisor/{role}", (string username, string role) => companyCache.PlayerCompanies[userCache.Users[username].CompanyId].Advisors[role].Describe());
//app.MapGet("/company/{username}/spendtp/{tp}", (string username, double tp) => APICalls.SpendTp(username, tp, userCache, companyCache));
#endregion

app.Run();
