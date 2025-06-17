namespace ColegioLiceu.Core.Interfaces.Helpers;

public interface IEncryption
{
    string Encrypt(object value);
    string Decrypt(string value);
    T Decrypt<T>(string value);
}
