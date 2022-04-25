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


EventStatus er = EventOperators.RunStage(company);
Console.WriteLine(er.ResultDescription);
foreach (EventOption option in er.Options)
    Console.WriteLine(option.Text);

Console.WriteLine("Starting stage2");
// TESTING HERE!!!   There is an error
company.EventStatus.PlayerChoice = 1;
er = EventOperators.RunStage(company);
Console.WriteLine(er.ResultDescription);

Console.WriteLine("Complete");
