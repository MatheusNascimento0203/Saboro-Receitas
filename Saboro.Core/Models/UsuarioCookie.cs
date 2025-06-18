using Saboro.Core.Enums;
using Saboro.Core.Extensions;

namespace Saboro.Core.Models;

public class UsuarioCookie
{
    public int Id { get; set; }
    public string NomeCompleto { get; set; }
    public string Email { get; set; }
    public bool ManterConectado { get; set; }


    public string PrimeiroNome => NomeCompleto?.Split(' ').FirstOrDefault();
    public string Sobrenome => NomeCompleto?.Split(' ').Length > 1 ? NomeCompleto?.Split(' ')[1] : "";

}
