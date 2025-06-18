using Saboro.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Saboro.Web.Controllers;

public class HomeController() : BaseController
{
    [HttpGet("home")]
    public IActionResult Index() => View();
    
    [HttpGet("logout")]
    public IActionResult Logout()
    {
        HttpContext.LogOut();
        return RedirectToAction("Index", "Login");
    }
}
