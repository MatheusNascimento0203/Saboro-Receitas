using Saboro.Core.Helpers;
using Saboro.Core.Interfaces.Helpers;

namespace Saboro.Web.ViewModels.Login;

public class RedefinirSenhaViewModel
{
    public string Email { get; set; }
    public string Senha { get; set; }
    public string SenhaConfirmacao { get; set; }
    public string Token { get; set; }

    public bool IsValidSenha(string senha, INotification notification)
    {
        if (string.IsNullOrWhiteSpace(senha) || senha.Length < 8)
        {
            notification.Add("Para sua segurança, a senha deve conter pelo menos 8 caracteres, incluindo letras, números e caracteres especiais.", NotificationType.Error);
            return false;
        }

        bool contemLetra = false, contemDigito = false, contemCaracterEspecial = false;
        foreach (var c in senha)
        {
            if (char.IsLetter(c)) contemLetra = true;
            else if (char.IsDigit(c)) contemDigito = true;
            else if (!char.IsLetterOrDigit(c)) contemCaracterEspecial = true;

        }

        notification.Add("Para sua segurança, a senha deve conter pelo menos 8 caracteres, incluindo letras, números e caracteres especiais.", NotificationType.Error);

        return contemLetra && contemDigito && contemCaracterEspecial;
    }
}
