using Microsoft.EntityFrameworkCore;
using Saboro.Core.Interfaces.Repositories;
using Saboro.Core.Models;
using Saboro.Data.Context;

namespace Saboro.Data.Repositories.Base;

public class ReceitaRepository(ApplicationDbContext dbContext) : BaseRepository(dbContext), IReceitaRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task AdicionarAsync(Receita receita)
    {
        await _dbContext.AddAsync(receita);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Receita>> BuscarReceitaAsync()
    {
        return await _dbContext.Receitas
            .Include(r => r.CategoriaFavorita)
            .Include(r => r.DificuldadeReceita)
            .Include(r => r.Ingredientes)
            .Include(r => r.ModoPreparoReceitas)
            .ToListAsync();
    }

}
