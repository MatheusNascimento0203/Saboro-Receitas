using System.Net;

namespace Saboro.Web.Configurations.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logWriter)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logWriter = logWriter;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            var route = context.Request.Path.ToString().ToLower().Trim('/');

            _logWriter.LogError($"Erro durante a chamada da rota: {route} - EXCEPTION: {ex}");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync("Ocorreu um erro inesperado. Contate o Administrador!");
        }
    }
}