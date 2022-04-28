global using Company;
global using People;
global using Relation;
global using Users;
global using FileTools;
global using Events;
global using APIMethods;

Boolean NewData = false;

Guid LoadToolGuid = new Guid("fd46a92d-5c61-4afa-b6bf-63876fae3a5c");
FileTool fileTool = await StartupTools.ConstructFileTool();
LoadTool loadTool = await fileTool.ReadLoadTool(LoadToolGuid);

CitizenCache citizenCache = await StartupTools.ConstructCitizenCache(NewData, loadTool, fileTool);
CompanyCache companyCache = await StartupTools.ConstructCompanyCache(NewData, loadTool, fileTool); 
UserCache userCache = await StartupTools.ConstructUserCache(NewData, loadTool, fileTool);
RelationshipCache relationshipCache = await StartupTools.ConstructRelationshipCache(NewData, loadTool, fileTool);

User user = userCache.Users["string"];
PlayerCompany company = companyCache.PlayerCompanies[user.CompanyId];

company.EventStatus = new();

company.EventStatus.NextStage = Event.TestEventStageOne;
user.TimePoints = 1000;

string returnString = ExplorationAPIMethods.Walk(user, company);  //EventOperators.RunStage(company);
Console.WriteLine(returnString);

company.EventStatus.PlayerChoice = 1;
returnString = ExplorationAPIMethods.ProgressEvent(company, 1);
Console.WriteLine("\n" + returnString);

Console.WriteLine("\nComplete");
