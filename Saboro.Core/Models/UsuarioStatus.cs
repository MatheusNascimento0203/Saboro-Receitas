namespace Saboro.Core.Models;

public class UsuarioStatus
{
    public int Id { get; set; }
    public string NomeStaus { get; set; }

    public ICollection<Usuario> Usuarios { get; set; }
}