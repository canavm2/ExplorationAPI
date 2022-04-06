using People;
using Company;
using Users;
using Relation;

namespace DependencyInjection
{
    public static class ServiceExtensions
    {
        public static void RegisterRepos(this IServiceCollection collection, CitizenCache citizenCache, CompanyCache companyCache, UserCache userCache, RelationshipCache relationshipCache)
        {
            collection.AddSingleton<ICitizenCache>(citizenCache);
            collection.AddSingleton<ICompanyCache>(companyCache);
            collection.AddSingleton<IUserCache>(userCache);
            collection.AddSingleton<IRelationshipCache>(relationshipCache);
            //Add other repositories
        }

        public static void RegisterLogging(this IServiceCollection collection)
        {
            //Register logging
        }

        public static void RegisterAuth(this IServiceCollection collection)
        {
            //Register authentication services.
        }
    }
}
