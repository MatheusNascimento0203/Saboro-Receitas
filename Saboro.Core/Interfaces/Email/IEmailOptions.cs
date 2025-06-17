using System.Text;
using Saboro.Core.Helpers.Email;
using Saboro.Core.Enums;

namespace Saboro.Core.Interfaces.Helpers.Email;

public interface IEmailOptions
{
    string Subject { get; set; }
    string Body { get; set; }
    EmailPriority Priority { get; set; }
    bool IsBodyHtml { get; set; }
    Encoding Encoding { get; set; }
    IEnumerable<string> CarbonCopies { get; set; }
    IEnumerable<EmailAttachment> Attachments { get; set; }
}
