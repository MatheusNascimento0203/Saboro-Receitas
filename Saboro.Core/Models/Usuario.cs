namespace Saboro.Core.Models;

public class Usuario
{
    public int Id { get; set; }
    public int IdCategoriaFavorita { get; set; }
    public int IdNivelCulinario { get; set; }
    public string NomeCompleto { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public int UsuarioCadastro { get; set; } 
    public DateTime DataCadastro { get; set; } = DateTime.Now;
    public int? UsuarioUltimaAlteracao { get; set; }
    public DateTime? DataUltimaAlteracao { get; set; }
    public DateTime? DataDesativacao { get; set; }
    public short TentativasInvalidas { get; set; }
    public string Biografia { get; set; }

    public CategoriaFavorita CategoriaFavorita { get; set; }
    public NivelCulinario NivelCulinario { get; set; }

}
