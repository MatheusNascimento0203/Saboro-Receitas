using System.Reflection;
using Saboro.Core.Models;
using Saboro.Core.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Saboro.Data.Context;

public class ApplicationDbContext(AppSettings appSettings, ILogger<ApplicationDbContext> logWriter)
    : BaseDbContext(appSettings, logWriter, Assembly.GetExecutingAssembly())
{
    public DbSet<NivelCulinario> NiveisCulinarios { get; set; }
    public DbSet<CategoriaFavorita> CategoriasFavoritas { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Receita> Receitas { get; set; }
    public DbSet<DificuldadeReceita> DificuldadesReceitas { get; set; }
}