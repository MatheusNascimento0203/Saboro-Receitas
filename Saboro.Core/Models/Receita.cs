namespace Saboro.Core.Models;

public class Receita
{
    public int Id { get; set; }
    public int IdUsuario { get; set; }
    public int IdDificuldadeReceita { get; set; }
    public int IdCategoriaFavorita { get; set; }
    public string TituloReceita { get; set; }
    public string DescricaoReceita { get; set; }
    public int TempoPreparo { get; set; }
    public int QtdPorcoes { get; set; }    
    public DateTime DataCadastro { get; set; } = DateTime.Now;
    public int? UsuarioUltimaAlteracao { get; set; }
    public DateTime? DataUltimaAlteracao { get; set; }

    public CategoriaFavorita CategoriaFavorita { get; set; }
    public  DificuldadeReceita DificuldadeReceita { get; set; }
    public  Usuario Usuario { get; set; }
    public  ICollection<IngredienteReceita> Ingredientes { get; set; }
    public  ICollection<ModoPreparoReceita> ModoPreparoReceitas { get; set; }

}
