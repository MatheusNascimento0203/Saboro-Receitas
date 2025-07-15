using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Saboro.Core.Extensions;
using Saboro.Core.Interfaces.Helpers;
using Saboro.Core.Interfaces.Repositories;
using Saboro.Core.Models;
using Saboro.Web.Extensions;
using Saboro.Web.ViewModels.Usuario;

namespace Saboro.Web.Controllers.Usuario;

[Route("usuario")]
public class UsuarioController(INotification notification, IUsuarioRepository usuarioRepository, ICategoriaFavoritaRepository categoriaFavoritaRepository, INivelCulinarioRepository nivelCulinarioRepository) : Controller
{
    private readonly INotification _notification = notification;
    private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;
    private readonly ICategoriaFavoritaRepository _categoriaFavoritaRepository = categoriaFavoritaRepository;
    private readonly INivelCulinarioRepository _nivelCulinarioRepository = nivelCulinarioRepository;

    [HttpGet("minha-conta")]
    public async Task<IActionResult> Index()
    {
        // Implementar lógica para exibir a lista de usuários ou detalhes do usuário
        var usuario = HttpContext.GetUser();
        if (usuario == null)
            return Unauthorized();

        var categoriasFavoritas = await _categoriaFavoritaRepository.BuscarCategoriaAsync();
        var niveisCulinarios = await _nivelCulinarioRepository.BuscarNivelCulinarioAsync();

        if (categoriasFavoritas == null || !categoriasFavoritas.Any())
            return BadRequest("Nenhuma categoria cadastrada.");

        if (niveisCulinarios == null || !niveisCulinarios.Any())
            return BadRequest("Nenhum nível culinário cadastrado.");

        ViewBag.CategoriaFavorita = categoriasFavoritas;
        ViewBag.NivelCulinario = niveisCulinarios;

        var usuarios = await _usuarioRepository.BuscarAsync(usuario.Id);
        return View("Index", usuarios);
    }

    [HttpGet("editar-perfil")]
    public async Task<IActionResult> GetEditarPerfil()
    {
        var usuario = HttpContext.GetUser();
        if (usuario == null)
            return Unauthorized();

        var usuarioCompleto = await _usuarioRepository.BuscarAsync(usuario.Id);
        return PartialView("_FormEditarPerfil", usuarioCompleto);
    }

    [HttpPost("editar-perfil-post")]
    public async Task<IActionResult> PostEditarPerfilAsync(UsuarioCadastroViewModel model)
    {
        if (model == null)
            return BadRequest("Usuário inválido.");

        if (!model.IsValid(notification))
            return BadRequest(_notification.GetAsString());

        var usuario = HttpContext.GetUser();
        if (usuario == null)
            return Unauthorized();

        var usuarioCompleto = await _usuarioRepository.BuscarAsync(usuario.Id);

        model.Id = usuario.Id;
        model.UsuarioUltimaAlteracao = usuario.Id;
        model.DataUltimaAlteracao = DateTime.Now;


        if (usuarioCompleto == null)
            return BadRequest("Usuário não encontrado.");

        model.Senha = usuarioCompleto.Senha;
        model.IdUsuarioStatus = usuarioCompleto.IdUsuarioStatus;

        await _usuarioRepository.AtualizarAsync(model.Id, model);
        return Ok("Usuário atualizado com sucesso!");
    }

    [HttpGet("alterar-senha")]
    public async Task<IActionResult> GetAlterarSenha()
    {
        var usuario = HttpContext.GetUser();
        if (usuario == null)
            return Unauthorized();

        var usuarioCompleto = await _usuarioRepository.BuscarAsync(usuario.Id);
        return PartialView("_FormAlterarSenha", usuarioCompleto);
    }

    [HttpPost("alterar-senha-post")]
    public async Task<IActionResult> PostAlterarSenhaAsync(AlterarSenhaViewModel model)
    {
        if (model == null)
            return BadRequest("Usuário inválido.");

        if (!model.IsValid(notification))
            return BadRequest(_notification.GetAsString());

        var usuario = HttpContext.GetUser();
        if (usuario == null)
            return Unauthorized();

        var usuarioCompleto = await _usuarioRepository.BuscarAsync(usuario.Id);

        if (usuarioCompleto == null)
            return BadRequest("Usuário não encontrado.");

        if (usuarioCompleto.Senha != model.Senha.ToMd5Hash())
            return BadRequest("A senha atual informada está incorreta.");

        await _usuarioRepository.AtualizarAsync(usuario.Id, new
        {
            Senha = model.NovaSenha.ToMd5Hash(),
            UsuarioUltimaAlteracao = usuario.Id,
            DataUltimaAlteracao = DateTime.Now
        });

        return Ok("Usuário atualizado com sucesso!");
    }

    [HttpPost("excluir-usuario")]
    public async Task<IActionResult> DeleteExcluirUsuarioAsync()
    {
        var usuario = HttpContext.GetUser();

        if (usuario == null)
            return BadRequest("Usuário não encontrado.");

        await _usuarioRepository.RemoverAsync(usuario.Id);
        return Ok("Usuário excluido com sucesso!");
    }
}

