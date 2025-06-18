using ColegioLiceu.Core.Helpers.Email;
using Saboro.Core.Helpers;
using Saboro.Core.Interfaces.Helpers;
using Saboro.Core.Interfaces.Helpers.Email;
using Saboro.Core.Interfaces.Repositories;
using Saboro.Core.Interfaces.Services;
using Saboro.Core.Settings;
using Saboro.Data.Context;
using Saboro.Data.Repositories;
using Saboro.Web.Services;

namespace Saboro.Web.Settings;

public static class DependencyInjection
{
    public static void AddDependencies(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddSingleton(appSettings);
        services.AddScoped<ApplicationDbContext>();
        services.AddScoped<IEmailHandler, EmailHandler>();
        services.AddScoped<IEncryption, Encryption>();
        services.AddScoped<INotification, Notification>();

        services.AddRepositories();
        services.AddServices();
    }
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddScoped<IUsuarioService, UsuarioService>();
    }
}

