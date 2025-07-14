using Microsoft.EntityFrameworkCore;
using Saboro.Core.Interfaces.Repositories;
using Saboro.Core.Models;
using Saboro.Data.Context;
using Saboro.Data.Repositories.Base;

namespace Saboro.Data.Repositories;

public class NivelCulinarioRepository(ApplicationDbContext dbContext) : BaseRepository(dbContext), INivelCulinarioRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    public async Task<IEnumerable<NivelCulinario>> BuscarNivelCulinarioAsync()
    {
        return await _dbContext.NiveisCulinarios.OrderBy(nv => nv.NomeNivel).ToListAsync();
    }
}
