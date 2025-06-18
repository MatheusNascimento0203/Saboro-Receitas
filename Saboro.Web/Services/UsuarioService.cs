using Saboro.Core;
using Saboro.Core.Enums;
using Saboro.Web.Extensions;
using Saboro.Core.Extensions;
using Saboro.Core.Helpers;
using Saboro.Core.Interfaces.Helpers;
using Saboro.Core.Interfaces.Repositories;
using Saboro.Core.Interfaces.Services;
using Saboro.Core.Models;
using Saboro.Core.Settings;
using Saboro.Core.Enums.TemplateEmail;
using Saboro.Core.Factories;
using Saboro.Core.Interfaces.Helpers.Email;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Saboro.Web.Services;

public class UsuarioService(
    IUsuarioRepository usuarioRepository,
    IEmailHandler emailHandler,
    AppSettings appSettings) : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;
    private readonly IEmailHandler _emailHandler = emailHandler;
    private readonly AppSettings _appSettings = appSettings;

    public async Task<string> ObterNomePeloIdAsync(int idUsuario)
    {
        var nomeUsuario = await _usuarioRepository.BuscarNomePeloIdAsync(idUsuario);

        return nomeUsuario?.NomeCompleto ?? string.Empty;
    }

    public async Task AdicionarUsuarioAsync(Usuario usuario)
    {

        try
        {
            await _usuarioRepository.AdicionarAsync(usuario);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Erro ao adicionar usuário.", ex);
        }
    }

    public string GerarSenhaAleatoria()
    {
        const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var senha = new char[8];
        var random = new Random();

        for (int i = 0; i < senha.Length; i++)
            senha[i] = caracteres[random.Next(caracteres.Length)];

        return new string(senha);
    }

    public async Task<bool> ValidarDadosAsync(Usuario usuario, INotification notification)
    {
        if (usuario != null)
        {
            if (await _usuarioRepository.EmailJaExistenteAsync(usuario.Email, usuario.Id))
                notification.Add("Email já cadastrado.", NotificationType.Error);
        }

        return !notification.Any();
    }

    public async Task EditarUsuarioAsync(Usuario usuario)
    {
        var idUsuario = usuario.Id;
        if (idUsuario <= 0)
            throw new ArgumentException("ID do usuário inválido", nameof(usuario));

        try
        {

            var usuarioModificado = new
            {
                usuario.Email,
                usuario.NomeCompleto,
                usuario.Biografia,
                usuario.IdCategoriaFavorita,
                usuario.IdNivelCulinario,
                usuario.IdUsuarioStatus,
                usuario.UsuarioUltimaAlteracao,
                DataUltimaAlteracao = DateTime.UtcNow,
            };


            await _usuarioRepository.AtualizarAsync(idUsuario, usuarioModificado);
        }
        catch
        {
            throw;
        }
    }
}
