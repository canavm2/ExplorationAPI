global using Company;
global using People;
global using Relation;
global using Users;
global using FileTools;
global using Events;
global using APIMethods;
global using ExplorationAPI.Services.LoginService;

Boolean NewData = false;
Boolean FromCloud = true;
Boolean CreateLocal = false;

#region Cloud
if (FromCloud)
{
    // LOAD from Cloud
    Guid LoadToolGuid = new Guid("6462196d-7001-4304-8655-4f47c09630d6");
    FileTool fileTool = await StartupTools.ConstructFileTool();
    LoadTool loadTool = await fileTool.ReadLoadTool(LoadToolGuid);

    CitizenCache citizenCache = await StartupTools.ConstructCitizenCache(NewData, loadTool, fileTool);
    CompanyCache companyCache = await StartupTools.ConstructCompanyCache(NewData, loadTool, fileTool);
    UserCache userCache = await StartupTools.ConstructUserCache(NewData, loadTool, fileTool);

    // TEST from Cloud
    //UserDto userDto = new();
    //UserDto request = new();
    //request.UserName = "string";
    //request.Password = "string";
    //LoginService loginService = new LoginService();
    //loginService.CreatePasswordHash(request, out byte[] hash, out byte[] salt);
    //userCache.CreateNewUser(request.UserName, hash, salt, citizenCache, companyCache);
    User user = userCache.Users["string"];

    PlayerCompany company = companyCache.PlayerCompanies[user.CompanyId];

    #region Event Testing
    bool EventTesting = false;
    if (EventTesting)
    {
        company.EventStatus = new();
        //company.Master.SetSkillDev(Skill.Carpentry, 21);
        //company.Advisors[0].SetSkillDev(Skill.Tinker, 31);
        company.EventStatus.NextStage = Event.BrokenCartOne;

        string returnString = ExplorationAPIMethods.Walk(user, company);  //EventOperators.RunStage(company);
        Console.WriteLine(returnString);

        company.EventStatus.PlayerChoice = 0;
        returnString = ExplorationAPIMethods.ProgressEvent(company, company.EventStatus.PlayerChoice);
        Console.WriteLine("\n" + returnString);

        Console.WriteLine("\nComplete");
    }

    #endregion
}
#endregion

#region CreateLocal
if (CreateLocal)
{
    CitizenCache citizenCache = new CitizenCache(1);
    Console.WriteLine(citizenCache.FemaleCitizens[0].Describe());
}
#endregion