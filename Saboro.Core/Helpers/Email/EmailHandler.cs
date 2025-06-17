using System.Net;
using System.Net.Mail;
using System.Text;
using Saboro.Core.Enums;
using Saboro.Core.Interfaces.Helpers.Email;
using Saboro.Core.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Saboro.Core.Settings;
using Saboro.Core.Helpers.Email;

namespace ColegioLiceu.Core.Helpers.Email;

public class EmailHandler : IEmailHandler, IDisposable
{
    private EmailSettings _emailSetting;
    private SmtpClient _smtpClient;
    private readonly IWebHostEnvironment _env;

    public EmailHandler(AppSettings appSettings, IWebHostEnvironment env)
    {
        _smtpClient = new SmtpClient();
        _env = env;
        SetSettings(appSettings.Email);
    }

    public void SetSettings(EmailSettings setting)
    {
        _emailSetting = setting;
        _smtpClient = new SmtpClient(_emailSetting.Host, _emailSetting.Port)
        {
            EnableSsl = _emailSetting.EnableSsl,
            Credentials = new NetworkCredential(_emailSetting.Username, _emailSetting.Password),
            Timeout = (_emailSetting.Timeout ?? 0) * 1000
        };
    }

    public IEmailOptions Build(string subject, string body)
    {
        return Build(subject, body, EmailPriority.Normal, true, null);
    }

    public IEmailOptions Build(string subject, string body, EmailPriority priority, bool isBodyHtml, Encoding encoding)
    {
        return new EmailOptions
        {
            Subject = subject,
            Body = body,
            Priority = priority,
            IsBodyHtml = isBodyHtml,
            Encoding = encoding ?? Encoding.UTF8
        };
    }

    public async Task<IEmailOptions> BuildTemplate(string subject, string template, object replaces, EmailPriority priority = EmailPriority.Normal)
    {
        var body = await File.ReadAllTextAsync(template);

        foreach (var prop in replaces.GetType().GetProperties())
            body = body.Replace($"{{{prop.Name}}}", prop.GetValue(replaces)?.ToString());

        return Build(subject, body, priority, true, null);
    }

    public async Task SendAsync(string to, IEmailOptions options)
    {
        await SendAsync(new[] { to }, options);
    }

    public async Task SendAsync(IEnumerable<string> to, IEmailOptions options)
    {
        var from = !string.IsNullOrEmpty(_emailSetting.From) ? _emailSetting.From : _emailSetting.Username;
      
        if (!_env.IsProduction() && _emailSetting.EmailsDevelopment != null)
            to = _emailSetting.EmailsDevelopment;
        
        using (var mailMessage = new MailMessage())
        {
            mailMessage.From = new MailAddress(from);
            foreach (var address in to)
                mailMessage.To.Add(new MailAddress(address));

            mailMessage.Subject = options.Subject;
            mailMessage.Body = options.Body;
            mailMessage.IsBodyHtml = options.IsBodyHtml;
            mailMessage.BodyEncoding = options.Encoding;
            mailMessage.Priority = GetPriority(options.Priority);

            if (options.CarbonCopies != null)
                foreach (var cc in options.CarbonCopies)
                    mailMessage.CC.Add(new MailAddress(cc));
            

            if (options.Attachments != null)
                foreach (var attachment in options.Attachments)
                {
                    var stream = new MemoryStream(attachment.Buffer);
                    var mailAttachment = new Attachment(stream, attachment.Name, attachment.ContentType);
                    mailMessage.Attachments.Add(mailAttachment);
                }

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }

    private MailPriority GetPriority(EmailPriority priority)
    {
        return priority switch
        {
            EmailPriority.Low => MailPriority.Low,
            EmailPriority.High => MailPriority.High,
            _ => MailPriority.Normal,
        };
    }

    public void Dispose()
    {
        _smtpClient?.Dispose();
        _smtpClient = null;
        GC.SuppressFinalize(this);
    }
}
