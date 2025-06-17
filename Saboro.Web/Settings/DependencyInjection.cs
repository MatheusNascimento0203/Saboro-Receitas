using Saboro.Core.Helpers;
using Saboro.Core.Interfaces.Helpers;
using Saboro.Core.Interfaces.Repositories;
using Saboro.Core.Settings;
using Saboro.Data.Context;
using Saboro.Data.Repositories;

namespace Saboro.Web.Settings;

public static class DependencyInjection
{
    public static void AddDependencies(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddSingleton(appSettings);
        services.AddScoped<ApplicationDbContext>();
        services.AddScoped<INotification, Notification>();

        services.AddRepositories();
    }
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

    }
}

