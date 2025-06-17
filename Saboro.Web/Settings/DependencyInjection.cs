using Saboro.Core.Settings;

namespace SmnRun.Web.Settings;

public static class DependencyInjection
{
    public static void AddDependencies(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddSingleton(appSettings);
        services.AddScoped<ApplicationDbContext>();
    }

}