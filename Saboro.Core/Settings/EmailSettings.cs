namespace Saboro.Core.Settings;

public class EmailSettings
{
    public string Host { get; set; }
    public short Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string From { get; set; }
    public int? Timeout { get; set; }
    public bool EnableSsl { get; set; }
    public List<string> EmailsDevelopment { get; set; }
}
