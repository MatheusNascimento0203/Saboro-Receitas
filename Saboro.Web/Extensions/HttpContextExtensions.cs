using Saboro.Core.Extensions;
using Saboro.Core.Factories;
using Saboro.Core.Models;
using Saboro.Core.Settings;
using Saboro.Web.Helpers;
using Newtonsoft.Json;

namespace Saboro.Web.Extensions;

public static class HttpContextExtensions
{
    private static readonly string AppSettingsError = "Não foi possivel recuperar as configurações da aplicação";

    public static void LogIn(this HttpContext httpContext, UsuarioCookie user)
    {
        var settings = httpContext.RequestServices.GetService<AppSettings>()
            ?? throw new Exception(AppSettingsError);

        var json = user.ToJson(JsonFactory.DefaultSettings());
        var jwt = JwtHelper.Create(settings.Web.CookieKey, json, settings.Web.CookieDurationInHours);
        var token = jwt.Compress();
        var durantionCookie = DateTime.UtcNow.AddHours(settings.Web.CookieDurationInHours);

        if (user.ManterConectado)
            durantionCookie.AddDays(30);

        httpContext.Response.Cookies.Append(settings.Web.CookieName, token, new CookieOptions
        {
            Domain = settings.Web.CookieDomain,
            Expires = durantionCookie
        });

    }

    public static void LogOut(this HttpContext httpContext, UsuarioCookie user = null)
    {
        var settings = httpContext.RequestServices.GetService<AppSettings>()
            ?? throw new Exception(AppSettingsError);

        httpContext.Response.Cookies.Delete(settings.Web.CookieName, new CookieOptions
        {
            Domain = settings.Web.CookieDomain,
            Expires = DateTime.Now.AddDays(-1)
        });
    }

    public static UsuarioCookie GetUser(this HttpContext httpContext)
    {
        if (httpContext == null || httpContext.Request == null || httpContext.RequestServices == null)
            return null;

        var settings = httpContext.RequestServices.GetService<AppSettings>();
        if (settings == null || settings.Web == null)
            return null;

        if (httpContext.Request.Cookies == null)
            return null;

        var token = httpContext.Request.Cookies[settings.Web.CookieName];
        if (string.IsNullOrEmpty(token))
            return null;

        try
        {
            var value = JwtHelper.Read(token.Decompress(), settings.Web.CookieKey);
            if (string.IsNullOrEmpty(value))
                return null;

            var user = JsonConvert.DeserializeObject<UsuarioCookie>(value);
            if (user == null)
                return null;

            return user;
        }
        catch
        {
            return null;
        }
    }

    public static string GetBaseUrl(this HttpContext httpContext)
    {
        var request = httpContext.Request;
        return $"{request.Scheme}://{request.Host}{request.PathBase}".TrimEnd('/');
    }

    public static bool IsAjaxRequest(this HttpContext httpContext)
    {
        return httpContext.Request.Headers.XRequestedWith == "XMLHttpRequest";
    }
}
