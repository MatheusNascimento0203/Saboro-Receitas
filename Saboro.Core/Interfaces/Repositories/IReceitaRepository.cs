using Saboro.Core.Models;

namespace Saboro.Core.Interfaces.Repositories;

public interface IReceitaRepository
{
    Task<IEnumerable<Receita>> BuscarReceitaAsync();
    Task<Receita> BuscarReceitaPorIdAsync(int id);
    Task<IEnumerable<Receita>> BuscarReceitaPorUsuarioAsync(int idUsuario);
    Task AdicionarAsync(Receita receita);
    Task AtualizarAsync(int id, Receita receita);
    Task RemoverAsync(int id);
}
