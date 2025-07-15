using Saboro.Core.Helpers;
using Saboro.Core.Interfaces.Helpers;
using Saboro.Core.Models;

namespace Saboro.Web.ViewModels.Usuario;

public class UsuarioCadastroViewModel
{
    public int Id { get; set; }
    public int? IdCategoriaFavorita { get; set; } = null;
    public int? IdNivelCulinario { get; set; } = null;
    public int IdUsuarioStatus { get; set; }
    public string NomeCompleto { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public int? UsuarioUltimaAlteracao { get; set; }
    public DateTime? DataUltimaAlteracao { get; set; }
    public DateTime? DataDesativacao { get; set; }
    public string Biografia { get; set; } = null;

    public CategoriaFavorita CategoriaFavorita { get; set; }
    public NivelCulinario NivelCulinario { get; set; }

    public Saboro.Core.Models.Usuario ToModel() => new()
    {
        Id = Id,
        IdCategoriaFavorita = IdCategoriaFavorita,
        IdNivelCulinario = IdNivelCulinario,
        IdUsuarioStatus = IdUsuarioStatus,
        NomeCompleto = NomeCompleto,
        Email = Email,
        UsuarioUltimaAlteracao = UsuarioUltimaAlteracao,
        DataUltimaAlteracao = DataUltimaAlteracao,
        DataDesativacao = DataDesativacao,
        Biografia = Biografia        
    };

    public bool IsValid(INotification notification)
    {
        if (string.IsNullOrEmpty(NomeCompleto))
            notification.Add("Obrigatório informar o nome completo", NotificationType.Error);

        if (string.IsNullOrEmpty(Email))
            notification.Add("Obrigatório informar o e-mail", NotificationType.Error);       

        return !notification.Any();
    }
}
