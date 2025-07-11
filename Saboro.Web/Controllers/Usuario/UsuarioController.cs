using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Saboro.Core.Interfaces.Helpers;
using Saboro.Core.Interfaces.Repositories;
using Saboro.Web.Extensions;

namespace Saboro.Web.Controllers.Usuario;

[Route("usuario")]
public class UsuarioController(INotification notification, IUsuarioRepository usuarioRepository) : Controller
{
    private readonly INotification _notification = notification;
    private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;

    [HttpGet("minha-conta")]
    public async Task<IActionResult> Index()
    {
        // Implementar lógica para exibir a lista de usuários ou detalhes do usuário
        var usuario = HttpContext.GetUser();
        if (usuario == null)
            return Unauthorized();

        var usuarios = await _usuarioRepository.BuscarAsync(usuario.Id);
        // var receitasOrdenadas = receitas.OrderBy(r => r.TituloReceita).ToList();
        return View("Index", usuarios);
    }

    // Outros métodos relacionados ao usuário podem ser adicionados aqui
}

