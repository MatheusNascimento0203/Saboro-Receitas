using System.Reflection;
using Saboro.Core.Models;
using Saboro.Core.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Saboro.Data.Context;

public class ApplicationDbContext(AppSettings appSettings, ILogger<ApplicationDbContext> logWriter)
    : BaseDbContext(appSettings, logWriter, Assembly.GetExecutingAssembly())
{

}