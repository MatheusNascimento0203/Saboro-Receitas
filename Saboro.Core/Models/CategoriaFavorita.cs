namespace Saboro.Core.Models;

public class CategoriaFavorita
{
    public int Id { get; set; }
    public string NomeCategoria { get; set; }

    public ICollection<Usuario> Usuarios { get; set; }
}
