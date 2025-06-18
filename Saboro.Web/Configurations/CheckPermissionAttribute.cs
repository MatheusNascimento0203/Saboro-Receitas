using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Saboro.Web.Extensions;
using Saboro.Core.Settings;
using Saboro.Web.Controllers;

namespace Saboro.Web.Configurations;

public class CheckAuthenticatedAttribute : ActionFilterAttribute
{
    public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var usuario = context.HttpContext.GetUser();
        var _linkGenerator = context.HttpContext.RequestServices.GetService<LinkGenerator>();
        var _appSettings = context.HttpContext.RequestServices.GetService<AppSettings>();

        if (usuario == null)
        {
            context.Result = new RedirectResult(_linkGenerator.GetUriByAction(context.HttpContext,
                nameof(LoginController.Index), "Login", host: new HostString(_appSettings.Dominio)));
            return Task.CompletedTask;
        }

        return base.OnActionExecutionAsync(context, next);
    }
}
