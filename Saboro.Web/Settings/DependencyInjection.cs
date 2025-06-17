using Saboro.Core.Settings;
using Saboro.Data.Context;

namespace Saboro.Web.Settings;

public static class DependencyInjection
{
    public static void AddDependencies(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddSingleton(appSettings);
        services.AddScoped<ApplicationDbContext>();
    }

}