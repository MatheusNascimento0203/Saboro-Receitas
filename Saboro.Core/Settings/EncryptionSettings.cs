namespace Saboro.Core.Settings;

public class EncryptionSettings
{
    public string Password { get; set; }
    public string Salt { get; set; }
    public int Iterations { get; set; }
}
