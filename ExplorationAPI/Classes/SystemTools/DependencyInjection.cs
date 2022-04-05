using People;

namespace DependencyInjection
{
    public static class ServiceExtensions
    {
        public static void RegisterRepos(this IServiceCollection collection)
        {
            collection.AddSingleton<ICitizenCache, CitizenCache>();
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
