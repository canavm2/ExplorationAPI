global using Company;
global using People;
global using Relation;
global using Users;
global using FileTools;
global using Events;

Boolean NewData = false;

Guid LoadToolGuid = new Guid("fd46a92d-5c61-4afa-b6bf-63876fae3a5c");
FileTool fileTool = await StartupTools.ConstructFileTool();
LoadTool loadTool = await fileTool.ReadLoadTool(LoadToolGuid);

CitizenCache citizenCache = await StartupTools.ConstructCitizenCache(NewData, loadTool, fileTool);
CompanyCache companyCache = await StartupTools.ConstructCompanyCache(NewData, loadTool, fileTool); 
UserCache userCache = await StartupTools.ConstructUserCache(NewData, loadTool, fileTool);
RelationshipCache relationshipCache = await StartupTools.ConstructRelationshipCache(NewData, loadTool, fileTool);

User user = userCache.Users["mikecanavan"];
PlayerCompany company = companyCache.PlayerCompanies[user.CompanyId];
Console.WriteLine(company.Name);

company.EventStatus = new();

Console.WriteLine("here");
Console.WriteLine(company.EventStatus.InEvent);

company.EventStatus.NextStage = Event.TestEventStageOne;


string returnString = EventOperators.RunStage(company);
Console.WriteLine(returnString);

company.EventStatus.PlayerChoice = 1;
returnString = EventOperators.RunStage(company);
Console.WriteLine(returnString);

Console.WriteLine("Complete");
