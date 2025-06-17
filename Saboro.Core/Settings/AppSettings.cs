namespace Saboro.Core.Settings;

public class AppSettings
{
    public DatabaseSettings Database { get; set; }
    public string Dominio { get; set; }
    public EncryptionSettings Encryption { get; set; }
    public EmailSettings Email { get; set; }
    public bool HttpsRedirection { get; set; }
    public WebSettings Web { get; set; }
}

