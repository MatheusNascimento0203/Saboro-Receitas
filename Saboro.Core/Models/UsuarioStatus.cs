namespace Saboro.Core.Models;

public class UsuarioStatus
{
    public short Id { get; set; }
    public string Nome { get; set; }

    public ICollection<Usuario> Usuarios { get; set; }
}