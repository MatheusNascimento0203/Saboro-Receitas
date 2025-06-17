using System.Text;
using Saboro.Core.Interfaces.Helpers.Email;
using Saboro.Core.Enums;

namespace Saboro.Core.Helpers.Email;

public class EmailOptions : IEmailOptions
{
    public string Subject { get; set; }
    public string Body { get; set; }
    public EmailPriority Priority { get; set; }
    public bool IsBodyHtml { get; set; }
    public Encoding Encoding { get; set; }
    public IEnumerable<string> CarbonCopies { get; set; }
    public IEnumerable<EmailAttachment> Attachments { get; set; }
}
