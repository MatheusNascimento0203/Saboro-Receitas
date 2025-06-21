using Saboro.Core.Models;

namespace Saboro.Core.Interfaces.Repositories;

public interface IDificuldadeReceitaRepository
{
    Task<IEnumerable<DificuldadeReceita>> BuscarDificuldadeAsync();
}
