namespace Saboro.Core.Models;

public class DificuldadeReceita
{
    public int Id { get; set; }
    public string Dificuldade { get; set; }

    public ICollection<Receita> Receitas { get; set; }
}
