using Saboro.Core.Interfaces.Helpers;
using Saboro.Core.Settings;
using System.Security.Cryptography;
using System.Text;
 
namespace Saboro.Core.Helpers;
 
public class Encryption : IEncryption
{
    private readonly EncryptionSettings _setting;
    private readonly Aes _aes;

    public Encryption(AppSettings appSettings)
    {
        _setting = appSettings.Encryption;

        _aes = Aes.Create();
        var rfc2898DeriveBytes = new Rfc2898DeriveBytes(_setting.Password, 
            Encoding.ASCII.GetBytes(_setting.Salt), 
            _setting.Iterations, 
            HashAlgorithmName.SHA256);

        _aes.Key = rfc2898DeriveBytes.GetBytes(_aes.KeySize / 8);
        _aes.IV = rfc2898DeriveBytes.GetBytes(_aes.BlockSize / 8);
    }

    public string Encrypt(object value)
    {
        if (value == null)
            return null;
 
        ICryptoTransform encryptor = _aes.CreateEncryptor(_aes.Key, _aes.IV);
        byte[] encryptedBytes;
        using (var msEncrypt = new MemoryStream())
        {
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(value.ToString());
                csEncrypt.Write(plainBytes, 0, plainBytes.Length);
            }
            encryptedBytes = msEncrypt.ToArray();
        }
 
        return Convert.ToBase64String(encryptedBytes).Replace('/', '-');
    }
 
    public string Decrypt(string value)
    {
        ICryptoTransform decryptor = _aes.CreateDecryptor(_aes.Key, _aes.IV);
        byte[] decryptedBytes;
        var texto = Convert.FromBase64String(value.Replace('-', '/'));
        using (var msDecrypt = new MemoryStream(texto))
        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
        using (var msPlain = new MemoryStream())
        {
            csDecrypt.CopyTo(msPlain);
            decryptedBytes = msPlain.ToArray();
        }
 
        return Encoding.UTF8.GetString(decryptedBytes);
    }
 
    public T Decrypt<T>(string value)
    {
        if (string.IsNullOrEmpty(value))
            return default;
 
        var typeofValue = typeof(T);
        var nonNullableType = Nullable.GetUnderlyingType(typeofValue);
        var conversionType = nonNullableType ?? typeofValue;
 
        return (T)Convert.ChangeType(Decrypt(value), conversionType);
    }
}
