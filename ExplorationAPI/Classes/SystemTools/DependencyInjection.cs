using People;
using Company;
using Users;
using Relation;
using FileTools;
using ExplorationAPI.Services.UserServices;
using ExplorationAPI.Services.LoginService;

namespace DependencyInjection
{
    public static class ServiceExtensions
    {
        public static void RegisterRepos(this IServiceCollection collection,
            CitizenCache citizenCache,
            CompanyCache companyCache,
            UserCache userCache,
            FileTool fileTool)
        {
            collection.AddSingleton<ICitizenCache>(citizenCache);
            collection.AddSingleton<ICompanyCache>(companyCache);
            collection.AddSingleton<IUserCache>(userCache);
            collection.AddSingleton<IFileTool>(fileTool);
            //Add other repositories
        }

        public static void RegisterLogging(this IServiceCollection collection)
        {
            //Register logging
        }

        public static void RegisterAuth(this IServiceCollection collection)
        {
            collection.AddScoped<IUserService, UserService>();
            collection.AddScoped<ILoginService, LoginService>();
            collection.AddHttpContextAccessor();
            //Register authentication services.
        }
    }
}
