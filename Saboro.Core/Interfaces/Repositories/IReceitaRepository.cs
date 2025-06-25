using Saboro.Core.Models;

namespace Saboro.Core.Interfaces.Repositories;

public interface IReceitaRepository
{
    Task<IEnumerable<Receita>> BuscarReceitaAsync();
    // Task<Receita> BuscarReceitaPorIdAsync(int id);
    Task AdicionarAsync(Receita receita);
    // Task AtualizarAsync(int id, object modifiedFields);
    // Task RemoverAsync(int id);
}
