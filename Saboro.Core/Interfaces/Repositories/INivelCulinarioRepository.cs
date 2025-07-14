using Saboro.Core.Models;

namespace Saboro.Core.Interfaces.Repositories;

public interface INivelCulinarioRepository
{
    Task<IEnumerable<NivelCulinario>> BuscarNivelCulinarioAsync();
}
