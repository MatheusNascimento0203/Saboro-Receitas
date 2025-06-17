using System.Globalization;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace Saboro.Core.Extensions;

public static partial class StringExtensions
{
    [GeneratedRegex(@"\D")]
    private static partial Regex MyRegex();

    public static string ExtractNumbers(this string value) => MyRegex().Replace(value, string.Empty);

    public static string ToMd5Hash(this string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException(nameof(value));

        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(value));

        var sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
            sb.Append(hash[i].ToString("X2"));

        return sb.ToString().ToLower();
    }

    // https://stackoverflow.com/a/17993002/6093484
    public static string Compress(this string value)
    {
        var buffer = Encoding.UTF8.GetBytes(value);

        var memoryStream = new MemoryStream();
        using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            gZipStream.Write(buffer, 0, buffer.Length);

        memoryStream.Position = 0;

        var compressedData = new byte[memoryStream.Length];
        memoryStream.Read(compressedData, 0, compressedData.Length);

        var gZipBuffer = new byte[compressedData.Length + 4];
        Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
        Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
        return Convert.ToBase64String(gZipBuffer);
    }

    public static string ToCpf(this string value, string defaultValue = "")
    {
        if (string.IsNullOrWhiteSpace(value))
            return defaultValue;

        if (long.TryParse(value.ExtractNumbers(), out long castedValue))
            return castedValue.ToCpf(defaultValue);

        return defaultValue;
    }
    
    public static string ToCpf(this long value, string defaultValue = "")
    {
        if (value <= 0)
            return defaultValue;
            
        var cpf = value.ToString().PadLeft(11, '0');
        return Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
    }
    
    public static string FormatarCep(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;
        
        var cepNumeros = value.ExtractNumbers();
        
        if (cepNumeros.Length == 8)
            return cepNumeros.Substring(0, 5) + "-" + cepNumeros.Substring(5, 3);
        else
            return value;
    }

    public static string FormatarTelefone(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;
        
        var numerosTelefone = value.ExtractNumbers();
        
        if (numerosTelefone.Length == 10)
            return $"({numerosTelefone.Substring(0, 2)}) {numerosTelefone.Substring(2, 4)}-{numerosTelefone.Substring(6, 4)}";
        else
            return value;
    }

    public static string FormatarCelular(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;
        
        var numerosCelular = value.ExtractNumbers();
        
        if (numerosCelular.Length == 11)
            return $"({numerosCelular.Substring(0, 2)}) {numerosCelular.Substring(2, 5)}-{numerosCelular.Substring(7, 4)}";
        else
            return value;
    }

    public static bool IsValidCep(this string value)
    {
        var RgxFormatado = new Regex(@"^\d{5}-\d{3}$");
        var RgxSemFormatacao = new Regex(@"^\d{8}$");
        return RgxFormatado.IsMatch(value) || RgxSemFormatacao.IsMatch(value);
    }

    public static string Decompress(this string value)
    {
        try
        {
            var gZipBuffer = Convert.FromBase64String(value);
            using (var memoryStream = new MemoryStream())
            {
                var dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                    gZipStream.Read(buffer, 0, buffer.Length);

                return Encoding.UTF8.GetString(buffer);
            }
        }
        catch
        {
            return null;
        }
    }

    public static bool IsValidEmail(this string email)
    {
        if (string.IsNullOrEmpty(email?.Trim()))
            return false;

        string pattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";

        return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
    }

   public static bool IsValidCpf(this string value)
    {
        var cpf = value.ExtractNumbers().PadLeft(11, '0');

        var digitosIguais = true;
        for (var i = 0; i < cpf.Length - 1; i++)
            if (cpf[i] != cpf[i + 1])
            {
                digitosIguais = false;
                break;
            }

        if (digitosIguais)
            return false;

        var numeros = cpf.Substring(0, 9);
        var digitos = cpf.Substring(9);

        var soma = 0;
        for (var i = 10; i > 1; i--)
            soma += int.Parse(numeros.Substring(10 - i, 1)) * i;

        var resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != int.Parse(digitos.Substring(0, 1)))
            return false;

        numeros = cpf.Substring(0, 10);

        soma = 0;
        for (var i = 11; i > 1; i--)
            soma += int.Parse(numeros.Substring(11 - i, 1)) * i;

        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != int.Parse(digitos.Substring(1, 1)))
            return false;

        return true;
    }

    public static bool Like(this string value, params string[] texts)
    {
        if (texts == null || texts.Length == 0)
            return false;

        value = value.Trim();
        return texts.Any(text => value.Equals(text, StringComparison.OrdinalIgnoreCase));
    }

    public static int TryToInt(this string value) => int.TryParse(value, out int numero) ? numero : default;

    public static string RemoveDiacritics(this string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

        for (int i = 0; i < normalizedString.Length; i++)
        {
            char c = normalizedString[i];
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                stringBuilder.Append(c);
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

     public static string BuildMenuUrl(this IUrlHelper urlHelper, string menuName, string menuNameMae = null, bool hasChild = false)
        {
            if ( hasChild && string.IsNullOrWhiteSpace(menuNameMae))
                return "javascript:void(0)";

            if (hasChild == false && string.IsNullOrWhiteSpace(menuNameMae))
                return urlHelper.Action("Index", menuName.RemoveDiacritics().ToLower()); 

            var arrayMenuName = menuName.Split(" ").ToList();
           
            if (arrayMenuName.Count != 0)
                 arrayMenuName.RemoveAll(item => item.Length <= 2);

            var action =  string.Join("-", arrayMenuName).RemoveDiacritics().ToLower();
            var controller = menuNameMae.RemoveDiacritics().ToLower();

            var url = urlHelper.Action(action, controller);
            
            return url ?? action;
        }
}