using System.Text;
using Saboro.Core.Enums;
using Saboro.Core.Settings;

namespace Saboro.Core.Interfaces.Helpers.Email;

public interface IEmailHandler
{
    void SetSettings(EmailSettings setting);
    IEmailOptions Build(string subject, string body);
    IEmailOptions Build(string subject, string body, EmailPriority priority, bool isBodyHtml, Encoding encoding);
    Task<IEmailOptions> BuildTemplate(string subject, string template, object replaces, EmailPriority priority = EmailPriority.Normal);
    Task SendAsync(string to, IEmailOptions options);
    Task SendAsync(IEnumerable<string> to, IEmailOptions options);
}
