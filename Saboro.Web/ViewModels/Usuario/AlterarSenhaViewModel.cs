using Saboro.Core.Helpers;
using Saboro.Core.Interfaces.Helpers;

namespace Saboro.Web.ViewModels.Usuario;

public class AlterarSenhaViewModel
{
    public string Senha { get; set; }
    public string NovaSenha { get; set; }
    public string NovaSenhaConfirmacao { get; set; }
    public int UsuarioUltimaAlteracao { get; set; }
    public DateTime DataUltimaAlteracao { get; set; }

    public bool IsValid(INotification _notification)
    {
        bool valido = true;

        if (string.IsNullOrEmpty(Senha))
        {
            _notification.Add("Obrigatório informar a senha", NotificationType.Error);
            valido = false;
        }

        if (string.IsNullOrEmpty(NovaSenha))
        {
            _notification.Add("Obrigatório informar a nova senha", NotificationType.Error);
            valido = false;
        }

        if (string.IsNullOrEmpty(NovaSenhaConfirmacao))
        {
            _notification.Add("Obrigatório informar a confirmação da senha", NotificationType.Error);
            valido = false;
        }

        if (!string.IsNullOrEmpty(NovaSenha) && !NovaSenha.Equals(NovaSenhaConfirmacao))
        {
            _notification.Add("A senha e a confirmação de senha não conferem", NotificationType.Error);
            valido = false;
        }

        if (!string.IsNullOrWhiteSpace(NovaSenha))
        {
            if (NovaSenha.Length < 8)
            {
                _notification.Add("A nova senha deve conter pelo menos 8 caracteres.", NotificationType.Error);
                valido = false;
            }

            bool contemLetra = NovaSenha.Any(char.IsLetter);
            bool contemDigito = NovaSenha.Any(char.IsDigit);
            bool contemEspecial = NovaSenha.Any(c => !char.IsLetterOrDigit(c));

            if (!(contemLetra && contemDigito && contemEspecial))
            {
                _notification.Add("A nova senha deve conter letras, números e caracteres especiais.", NotificationType.Error);
                valido = false;
            }
        }

        return valido && !_notification.Any();
    }
}
