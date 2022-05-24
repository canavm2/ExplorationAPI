global using Company;
global using People;
global using Relation;
global using Users;
global using FileTools;
global using Events;
global using APIMethods;

Boolean NewData = false;
Boolean FromCloud = false;
Boolean CreateLocal = true;

#region Cloud
if (FromCloud)
{
    // LOAD from Cloud
    Guid LoadToolGuid = new Guid("fd46a92d-5c61-4afa-b6bf-63876fae3a5c");
    FileTool fileTool = await StartupTools.ConstructFileTool();
    LoadTool loadTool = await fileTool.ReadLoadTool(LoadToolGuid);

    CitizenCache citizenCache = await StartupTools.ConstructCitizenCache(NewData, loadTool, fileTool);
    CompanyCache companyCache = await StartupTools.ConstructCompanyCache(NewData, loadTool, fileTool);
    UserCache userCache = await StartupTools.ConstructUserCache(NewData, loadTool, fileTool);
    RelationshipCache relationshipCache = await StartupTools.ConstructRelationshipCache(NewData, loadTool, fileTool);

    // TEST from Cloud
    User user = userCache.Users["string"];
    PlayerCompany company = companyCache.PlayerCompanies[user.CompanyId];

    company.EventStatus = new();

    company.Advisors["master"].SetSkillDev(Skill.Carpentry, 21);
    company.Advisors["advisor1"].SetSkillDev(Skill.Tinker, 31);
    company.EventStatus.NextStage = Event.BrokenCartOne;

    string returnString = ExplorationAPIMethods.Walk(user, company);  //EventOperators.RunStage(company);
    Console.WriteLine(returnString);

    company.EventStatus.PlayerChoice = 0;
    returnString = ExplorationAPIMethods.ProgressEvent(company, company.EventStatus.PlayerChoice);
    Console.WriteLine("\n" + returnString);

    Console.WriteLine("\nComplete");
}
#endregion

#region CreateLocal
if (CreateLocal)
{
    CitizenCache citizenCache = new CitizenCache(1);
    Console.WriteLine(citizenCache.FemaleCitizens[0].Describe());
}
#endregion