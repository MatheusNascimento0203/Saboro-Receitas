using Saboro.Core.Models;

namespace Saboro.Core.Interfaces.Repositories;

public interface IReceitaRepository
{
    Task<IEnumerable<Receita>> BuscarReceitaAsync(string nome = null, bool buscaExata = false);
    Task<Receita> BuscarReceitaPorIdAsync(int id);
    Task<IEnumerable<Receita>> BuscarReceitaPorUsuarioAsync(int idUsuario, string nome = null);
    Task AdicionarAsync(Receita receita);
    Task AtualizarAsync(int id, object receita);
    Task AtualizarIngredientesAsync(int id, Receita receita);
    Task AtualizarModoPreparoAsync(int id, Receita receita);
    Task RemoverAsync(int id);
}
