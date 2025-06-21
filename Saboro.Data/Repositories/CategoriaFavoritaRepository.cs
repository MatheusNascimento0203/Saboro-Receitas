using Microsoft.EntityFrameworkCore;
using Saboro.Core.Interfaces.Repositories;
using Saboro.Core.Models;
using Saboro.Data.Context;
using Saboro.Data.Repositories.Base;

namespace Saboro.Data.Repositories;

public class CategoriaFavoritaRepository(ApplicationDbContext dbContext) : BaseRepository(dbContext), ICategoriaFavoritaRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    public async Task<IEnumerable<CategoriaFavorita>> BuscarCategoriaAsync()
    {
        return await _dbContext.CategoriasFavoritas.OrderBy(cf => cf.NomeCategoria).ToListAsync();
    }
}
