using Saboro.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Saboro.Web.Controllers;

public class HomeController() : BaseController
{
    [HttpGet("home")]
    public IActionResult Index()
    {
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
