namespace Saboro.Core.Models;

public class NivelCulinario
{
    public int Id { get; set; }
    public string NomeNivel { get; set; }

    public ICollection<Usuario> Usuarios { get; set; }
}
