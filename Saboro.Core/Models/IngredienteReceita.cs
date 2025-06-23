namespace Saboro.Core.Models;

public class IngredienteReceita
{
    public int Id { get; set; }
    public int IdReceita { get; set; }
    public string DescricaoIngrediente { get; set; }

    public Receita Receita { get; set; }
}
