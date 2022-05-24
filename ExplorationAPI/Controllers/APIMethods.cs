using People;
using Company;
using Users;
using Relation;
using FileTools;

namespace APIMethods;
public class APICalls
{
    public static string ReturnTest(string returnString)
    {
        return returnString;
    }
    public static string ReturnCitizen(CitizenCache cache)
    {
        Citizen citizen = cache.GetRandomCitizen();
        return citizen.Describe();
    }
    public static string ReturnCitizenFromCompany(PlayerCompany playercompany, int id)
    {
        if (id == 0) return playercompany.Advisors["master"].Describe();
        else if (id > 0 && id < 6)
        {
            string advisorkey = $"advisor{id}";
            return playercompany.Advisors[advisorkey].Describe();
        }
        else if (id >= 6 && id < playercompany.Advisors.Count)
        {
            string advisorkey = $"bench{id-6}";
            return playercompany.Advisors[advisorkey].Describe();
        }
        else return $"Error: id must be between 0 and {playercompany.Advisors.Count}";
    }
    public static async Task<string> AdvanceSave(FileTool fileTool, ICitizenCache citizenCache, UserCache userCache, CompanyCache companyCache, RelationshipCache relationshipCache)
    {
        DateTime currentDateTime = DateTime.Now;
        await fileTool.StoreCitizens((CitizenCache)citizenCache);
        await fileTool.StoreCompanies(companyCache);
        await fileTool.StoreRelationshipCache(relationshipCache);
        await fileTool.StoreUsers(userCache);
        return "Everything saved!  Beep Beep Woop Woop";
    }
    //public static string CreateUser(string userName, UserCache userCache, ICitizenCache citizenCache, CompanyCache companyCache)
    //{
    //    if (userCache.Users.ContainsKey(userName)) return "UserName already Exists, choose something else.";
    //    userCache.CreateNewUser(userName, citizenCache, companyCache);
    //    return "User Created, use your UserID in the API now.";
    //}
    public static string StandardInfo(string userName, UserCache userCache, CompanyCache companyCache)
    {
        //string standardInfo = userCache.Users[userName].Describe();
        //standardInfo += companyCache.PlayerCompanies[userCache.Users[userName].CompanyId].Describe();
        //return standardInfo;
        return String.Empty;
    }
    public static string SpendTp(string userName, int timePoints, UserCache userCache, CompanyCache companyCache)
    {
        Guid user = userCache.Users[userName].id;
        bool spent = companyCache.PlayerCompanies[user].TimeBlock.SpendTimePoints(timePoints);
        if (spent) return "You have spent your timepoint, I award you nothing.";
        else return "You did not have enough timepoints to spend, the more you tighten your grip the more star systems will slip through your fingers.";
    }
}
