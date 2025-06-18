using Saboro.Core.Interfaces.Repositories.Base;
using Saboro.Core.Models;

namespace Saboro.Core.Interfaces.Repositories;

public interface IUsuarioRepository : IBaseRepository
{
    Task<(IEnumerable<Usuario> Usuarios, int Total)> BuscarAsync(int? pagina = 1, int? tamanhoPagina = 7);
    Task<Usuario> BuscarAsync(int id);
    Task<Usuario> BuscarPorEmailAsync(string email);
    Task<Usuario> BuscarNomePeloIdAsync(int idUsuario);
    Task AtualizarAsync(int id, object modifiedFields);
    Task AdicionarAsync(Usuario usuario);
    Task RemoverAsync(Usuario usuario);
    Task<bool> EmailJaExistenteAsync(string email, int? idUsuarioAtual = null);
}
