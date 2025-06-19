using Saboro.Core.Helpers;
using Saboro.Core.Interfaces.Helpers;

namespace Saboro.Web.ViewModels.Usuario;

public class UsuarioViewModel
{
    public int Id { get; set; }
    public string NomeCompleto { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public string ConfirmarSenha { get; set; }

    public bool IsValid(INotification _notification)
    {
        if (string.IsNullOrEmpty(NomeCompleto))
            _notification.Add("Obrigatório informar o nome completo", NotificationType.Error);

        if (string.IsNullOrEmpty(Email))
            _notification.Add("Obrigatório informar o e-mail", NotificationType.Error);

        if (string.IsNullOrWhiteSpace(Senha) || Senha.Length < 8)
        {
            _notification.Add("Para sua segurança, a senha deve conter pelo menos 8 caracteres, incluindo letras, números e caracteres especiais.", NotificationType.Error);
            return false;
        }

        bool contemLetra = false, contemDigito = false, contemCaracterEspecial = false;
        foreach (var c in Senha)
        {
            if (char.IsLetter(c)) contemLetra = true;
            else if (char.IsDigit(c)) contemDigito = true;
            else if (!char.IsLetterOrDigit(c)) contemCaracterEspecial = true;

        }

        if (!(contemLetra && contemDigito && contemCaracterEspecial))
            _notification.Add("Para sua segurança, a senha deve conter letras, números e caracteres especiais.", NotificationType.Error);

        if (string.IsNullOrEmpty(ConfirmarSenha))
            _notification.Add("Obrigatório informar a confirmação da senha", NotificationType.Error);

        if (!string.IsNullOrEmpty(Senha) && !Senha.Equals(ConfirmarSenha))
            _notification.Add("A senha e a confirmação de senha não conferem", NotificationType.Error);

        return !_notification.Any();
    }
}
