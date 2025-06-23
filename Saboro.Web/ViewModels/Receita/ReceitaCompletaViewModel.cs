namespace Saboro.Web.ViewModels.Receitas;

public class ReceitaCompletaViewModel
{
    public ReceitaViewModel Receita { get; set; }
    public List<IngredienteReceitaViewModel> Ingredientes { get; set; }
    public List<ModoPreparoReceitaViewModel> ModosPreparo { get; set; }
    public string QueryString { get; set; }
}
