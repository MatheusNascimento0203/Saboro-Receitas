
using Microsoft.Extensions.DependencyInjection;
using Saboro.Data.Context;

namespace SmnRun.Data.Helpers;
public class Configuration
{
    public static void UpdateDbScripts(IServiceScope scope)
    {
        using var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>()
            ?? throw new Exception("Não foi possível recuperar uma instância de ApplicationDbContext");

        Console.WriteLine("Tentando fazer conexão com o banco de dados...");
        var canConnect = dbContext.Database.CanConnect();
        if (!canConnect)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Falha ao tentar se conectar com o banco de dados");
            return;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Conexão OK com o banco de dados");

        dbContext.UpdateScripts();
    }
}
