using Bogus;
using Bogus.Extensions.Brazil;
using Saboro.Core;

namespace Saboro.Tests.Saboro.Web.FakerData.ViewModels;

public class UsuarioViewFaker : Faker<UsuarioViewModel>
{
    public UsuarioFaker()
    {
        RuleFor(u => u.Id, f => f.Random.Int());
        RuleFor(u => u.Email, f => f.Person.Email);
        RuleFor(u => u.Senha, f => f.Random.String(8));
    }

    public static UsuarioViewModel GerarUsuarioViewModelValido()
    {
        return new UsuarioFaker().Generate();
    }

    public UsuarioViewModel GerarUsuarioViewModelInvalido()
    {
        var usuarioFakerInvalido = this.Clone();
        usuarioFakerInvalido.RuleFor(u => u.Email, f => "invalid-email");

        return usuarioFakerInvalido;
    }

}
