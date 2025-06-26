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

    public async Task<Receita> BuscarReceitaPorIdAsync(int id)
    {
        return await _dbContext.Receitas
            .Include(r => r.CategoriaFavorita)
            .Include(r => r.DificuldadeReceita)
            .Include(r => r.Ingredientes)
            .Include(r => r.ModoPreparoReceitas)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task RemoverAsync(int id)
    {
        var receita = await BuscarReceitaPorIdAsync(id);

        if (receita == null)
            throw new Exception("Receita não encontrada");

        _dbContext.Remove(receita);
        await _dbContext.SaveChangesAsync();

    }

}
