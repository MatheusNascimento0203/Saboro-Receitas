using Saboro.Core.Helpers;
using Saboro.Core.Interfaces.Helpers;
using Saboro.Core.Models;

namespace Saboro.Web.ViewModels.Receitas;

public class ReceitaViewModel
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
    public DificuldadeReceita DificuldadeReceita { get; set; }
    public Saboro.Core.Models.Usuario Usuario { get; set; }

    public Receita ToModel() => new()
    {
        Id = Id,
        IdUsuario = IdUsuario,
        IdDificuldadeReceita = IdDificuldadeReceita,
        IdCategoriaFavorita = IdCategoriaFavorita,
        TituloReceita = TituloReceita,
        DescricaoReceita = DescricaoReceita,
        TempoPreparo = TempoPreparo,
        QtdPorcoes = QtdPorcoes,
        DataCadastro = DataCadastro,
        UsuarioUltimaAlteracao = UsuarioUltimaAlteracao,
        DataUltimaAlteracao = DataUltimaAlteracao
    };

    public bool IsValid(INotification notification)
    {
        if (string.IsNullOrEmpty(TituloReceita))
            notification.Add("O preenchimento do campo Titulo da Receita é obrigatório ", NotificationType.Error);

        if (IdCategoriaFavorita <= 0)
            notification.Add("O preenchimento do campo Categoria da Receita é obrigatório ", NotificationType.Error);

        if (string.IsNullOrEmpty(DescricaoReceita))
            notification.Add("O preenchimento do campo Descricao da Receita é obrigatório ", NotificationType.Error);

        if (TempoPreparo <= 0)
            notification.Add("O preenchimento do campo Tempo de Preparo da Receita é obrigatório ", NotificationType.Error);

        if (QtdPorcoes <= 0)
            notification.Add("O preenchimento do campo Quantidade de Porcoes da Receita é obrigatório ", NotificationType.Error);

        if (IdDificuldadeReceita <= 0)
            notification.Add("O preenchimento do campo Dificuldade da Receita é obrigatório ", NotificationType.Error);

        return !notification.Any();
    }
}


