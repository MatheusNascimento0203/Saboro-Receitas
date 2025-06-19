using Saboro.Core.Helpers;
using Saboro.Core.Interfaces.Helpers;

namespace Saboro.Web.ViewModels.Login;

public class RedefinirSenhaViewModel
{
    public string Email { get; set; }
    public string Senha { get; set; }
    public string SenhaConfirmacao { get; set; }
    public int UsuarioUltimaAlteracao { get; set; }
    public DateTime DataUltimaAlteracao { get; set; }

    public bool IsValid(INotification _notification)
    {

        if (string.IsNullOrEmpty(Email))
            _notification.Add("Obrigatório informar o e-mail", NotificationType.Error);

        if (string.IsNullOrEmpty(SenhaConfirmacao))
            _notification.Add("Obrigatório informar a confirmação da senha", NotificationType.Error);

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

        _notification.Add("Para sua segurança, a senha deve conter pelo menos 8 caracteres, incluindo letras, números e caracteres especiais.", NotificationType.Error);

        if (!string.IsNullOrEmpty(Senha) && !Senha.Equals(SenhaConfirmacao))
            _notification.Add("A senha e a confirmação de senha não conferem", NotificationType.Error);

        return contemLetra && contemDigito && contemCaracterEspecial;
    }
}
