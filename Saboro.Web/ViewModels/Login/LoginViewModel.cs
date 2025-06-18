using Saboro.Core.Helpers;
using Saboro.Core.Interfaces.Helpers;

namespace Saboro.Web.ViewModels.Login;

public class LoginViewModel
{
    public string Email { get; set; }
    public string Senha { get; set; }
    public bool ManterConectado { get; set; }

    public bool IsValid(INotification _notification)
    {
        if (string.IsNullOrEmpty(Email))
            _notification.Add("Obrigatório informar o e-mail.", NotificationType.Error);

        if (string.IsNullOrEmpty(Senha))
            _notification.Add("Obrigatório informar a senha.", NotificationType.Error);

        return !_notification.Any();
    }
}
