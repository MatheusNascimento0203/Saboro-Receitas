using System.Globalization;
using Saboro.Core.Settings;
using Saboro.Web.Configurations.Middlewares;
using Saboro.Web.Settings;


var cultureInfo = new CultureInfo("pt-BR");
cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);

var appSettings = builder.Configuration.Get<AppSettings>();
builder.Configuration.Bind(appSettings);

builder.Services.AddDependencies(appSettings);
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<GeminiSettings>(builder.Configuration.GetSection("Gemini"));

var app = builder.Build();

using var scope = app.Services.CreateScope();


if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Home/Error");
app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    if (context.Request.Path.Value == "/")
    {
        context.Response.Redirect("/home");
        return;
    }
    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();