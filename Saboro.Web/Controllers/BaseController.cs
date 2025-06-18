using System.Net;
using Saboro.Core.Interfaces.Services;
using Saboro.Core.Models;
using Saboro.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace Saboro.Web.Controllers;

public class BaseController : Controller
{
    protected new UsuarioCookie User;
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        User = HttpContext.GetUser();

        if (User == null)
        {
            if (HttpContext.IsAjaxRequest())
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            else
                HttpContext.Response.Redirect("/login");

            return;
        }

        await next.Invoke();
    }
}
