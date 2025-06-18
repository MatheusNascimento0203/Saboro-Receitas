using Saboro.Core.Interfaces.Helpers;
using Saboro.Core.Models;

namespace Saboro.Core.Interfaces.Services;

public interface IUsuarioService
{
    Task<string> ObterNomePeloIdAsync(int idUsuario);
    Task AdicionarUsuarioAsync(Usuario usuario);
    Task EditarUsuarioAsync(Usuario usuario);
    Task<bool> ValidarDadosAsync(Usuario usuario, INotification notification);
    string GerarSenhaAleatoria();
}