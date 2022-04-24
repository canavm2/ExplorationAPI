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

TestEvent TestEvent = new("TestEvent");

company.Status = new();

Console.WriteLine("here");
Console.WriteLine(company.Status.InEvent);

Console.WriteLine("Advisor count: " + company.Advisors.Count);
Console.WriteLine("Advisor count: " + company.Status.EventResult.ResultDescription);

EventResult er = TestEvent.StageOne(company);
Console.WriteLine(er.ResultDescription);
foreach (EventOption option in er.Options)
    Console.WriteLine(option.Text);

Console.WriteLine("Advisor count: " + company.Advisors.Count);
Console.WriteLine("Advisor count: " + company.Status.EventResult.ResultDescription);
Console.WriteLine("Complete");
