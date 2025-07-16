using Saboro.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Saboro.Core.Interfaces.Repositories;
using System.Threading.Tasks;

namespace Saboro.Web.Controllers;

public class HomeController(IReceitaRepository receitaRepository) : BaseController
{
    private readonly IReceitaRepository _receitaRepository = receitaRepository;

    [HttpGet("home")]
    public async Task<IActionResult> Index()
    {
        var usuario = HttpContext.GetUser();
        if (usuario == null)
            return RedirectToAction("Index", "Login");

        var receitas = await _receitaRepository.BuscarReceitaPorUsuarioAsync(usuario.Id);
        if (receitas != null && receitas.Any())
            return RedirectToAction("Index", "Receita");

        DisableCache();
        return View("Index");
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {
        HttpContext.LogOut();
        return RedirectToAction("Index", "Login");
    }

    private void DisableCache()
    {
        Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
        Response.Headers["Pragma"] = "no-cache";
        Response.Headers["Expires"] = "0";
    }
}
