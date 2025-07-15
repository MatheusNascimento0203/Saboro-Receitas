using Saboro.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Saboro.Core.Interfaces.Repositories;

namespace Saboro.Web.Controllers;

public class HomeController(IReceitaRepository receitaRepository) : BaseController
{
    private readonly IReceitaRepository _receitaRepository = receitaRepository;

    [HttpGet("home")]
    public IActionResult Index()
    {
        var usuario = HttpContext.GetUser();
        if (usuario == null)
            return RedirectToAction("Index", "Login");

        var receitas = _receitaRepository.BuscarReceitaPorIdAsync(usuario.Id);
        if (receitas != null)
            return RedirectToAction("Index", "Receita");

        DisableCache();
        return View();
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
