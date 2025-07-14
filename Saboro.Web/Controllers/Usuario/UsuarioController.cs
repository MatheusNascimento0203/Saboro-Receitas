using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Saboro.Core.Interfaces.Helpers;
using Saboro.Core.Interfaces.Repositories;
using Saboro.Core.Models;
using Saboro.Web.Extensions;

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

}

