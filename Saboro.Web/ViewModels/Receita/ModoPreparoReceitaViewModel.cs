using Saboro.Core.Helpers;
using Saboro.Core.Interfaces.Helpers;
using Saboro.Core.Models;

namespace Saboro.Web.ViewModels.Receitas;

public class ModoPreparoReceitaViewModel
{
    public int Id { get; set; }
    public int IdReceita { get; set; }
    public int Ordem { get; set; }
    public string Descricao { get; set; }

    public Receita Receita { get; set; }

    public ModoPreparoReceita ToModel() => new()
    {
        IdReceita = IdReceita,
        Ordem = Ordem,
        Descricao = Descricao,
    };

    public bool IsValid(INotification notification)
    {
        if (IdReceita <= 0)
            notification.Add("O preenchimento do campo Receita é obrigatório ", NotificationType.Error);

        if (Ordem <= 0)
            notification.Add("O preenchimento do campo Ordem é obrigatório ", NotificationType.Error);

        if (string.IsNullOrEmpty(Descricao))
            notification.Add("O preenchimento do campo Descricao é obrigatório ", NotificationType.Error);

        return !notification.Any();
    }
}
