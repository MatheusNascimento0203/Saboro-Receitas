using Saboro.Core.Helpers;
using Saboro.Core.Interfaces.Helpers;
using Saboro.Core.Models;

namespace Saboro.Web.ViewModels.Receitas;

public class IngredienteReceitaViewModel
{
    public int Id { get; set; }
    public int IdReceita { get; set; }
    public string DescricaoIngrediente { get; set; }

    public Receita Receita { get; set; }

    public IngredienteReceita ToModel() => new()
    {
        IdReceita = IdReceita,
        DescricaoIngrediente = DescricaoIngrediente,
    };

    public bool IsValid(INotification notification)
    {
        if (string.IsNullOrEmpty(DescricaoIngrediente))
            notification.Add("O preenchimento do campo Ingrediente é obrigatório ", NotificationType.Error);

        return !notification.Any();
    }
}

