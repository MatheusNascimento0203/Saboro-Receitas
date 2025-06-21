namespace Saboro.Core.Models;

public class ModoPreparoReceita
{
    public int Id { get; set; }
    public int IdReceita { get; set; }
    public int Ordem { get; set; }
    public string Descricao { get; set; }

    public Receita Receita { get; set; }
}
