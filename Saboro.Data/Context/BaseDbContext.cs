using Saboro.Core.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;

namespace Saboro.Data.Context;

public class BaseDbContext(AppSettings appSetting, ILogger<BaseDbContext> logWriter) : DbContext
{
    private readonly AppSettings _appSetting = appSetting;
    private readonly ILogger<BaseDbContext> _logWriter = logWriter;
    private readonly Assembly _assembly;

    public BaseDbContext(AppSettings appSetting, ILogger<BaseDbContext> logWriter, Assembly assembly) : this(appSetting, logWriter)
    {
        _assembly = assembly;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (string.IsNullOrEmpty(_appSetting.Database?.ConnectionString))
            throw new Exception("Não foi possível recuperar a string de conexão com banco de dados");

        optionsBuilder.UseNpgsql(_appSetting.Database.ConnectionString, options =>
        {
            options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            options.CommandTimeout(60);
        });

        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);

        var logEnabled = _appSetting.Database?.EnableLog == true;

        if (Debugger.IsAttached || logEnabled)
            optionsBuilder.EnableDetailedErrors().EnableSensitiveDataLogging();

        void LogAction(string log)
        {
            _logWriter.LogInformation(log);
        }

        if (logEnabled)
            optionsBuilder.LogTo(LogAction, LogLevel.Information, DbContextLoggerOptions.SingleLine);
        else
            optionsBuilder.LogTo(LogAction, LogLevel.Error);

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (_assembly != null)
            UpdateConfigurations(modelBuilder, _assembly);

        base.OnModelCreating(modelBuilder);
    }

    private static void UpdateConfigurations(ModelBuilder modelBuilder, Assembly assembly)
    {
        var filter = $"{assembly.GetName().Name}.Configurations";
        modelBuilder.ApplyConfigurationsFromAssembly(assembly, x => x.Namespace != null && x.Namespace.Equals(filter));
    }

    public void UpdateScripts()
    {
        _logWriter.LogInformation($"Iniciando o processo sincronização dos scripts com o banco de dados");

        var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Scripts");
        if (!Directory.Exists(path))
        {
            _logWriter.LogInformation($"O diretório \"Scripts\" não foi encontrado");
            return;
        }

        var files = Directory.GetFiles(path, "*.sql", SearchOption.AllDirectories);
        if (files == null || !files.Any())
        {
            _logWriter.LogInformation($"Não foram encontrados scripts para sincronização com o banco de dados");
            return;
        }

        foreach (var file in files)
        {
            _logWriter.LogInformation($"Atualizando o script \"{Path.GetFileName(file)}\"", ConsoleColor.Green);
            var fileContent = File.ReadAllText(file);

            try
            {
                Database.ExecuteSqlRaw(fileContent);
            }
            catch (Exception ex)
            {
                _logWriter.LogError($"Erro ao executar o script \"{Path.GetFileName(file)}\": {ex}");
            }
        }
    }
}