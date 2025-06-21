using Saboro.Core.Models;

namespace Saboro.Core.Interfaces.Repositories;

public interface ICategoriaFavoritaRepository
{
    Task<IEnumerable<CategoriaFavorita>> BuscarCategoriaAsync();
}
