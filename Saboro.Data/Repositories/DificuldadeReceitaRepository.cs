using Microsoft.EntityFrameworkCore;
using Saboro.Core.Interfaces.Repositories;
using Saboro.Core.Models;
using Saboro.Data.Context;
using Saboro.Data.Repositories.Base;

namespace Saboro.Data.Repositories;

public class DificuldadeReceitaRepository(ApplicationDbContext dbContext) : BaseRepository(dbContext), IDificuldadeReceitaRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    public async Task<IEnumerable<DificuldadeReceita>> BuscarDificuldadeAsync()
    {
        return await _dbContext.DificuldadesReceitas.OrderBy(dr => dr.Dificuldade).ToListAsync();
    }
}
