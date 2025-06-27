using Bogus;
using Bogus.Extensions.Brazil;
using Saboro.Core;

namespace Saboro.Tests.Saboro.Web.FakerData.Models;

public class UsuarioFaker : Faker<UsuarioViewModel>
{
    public UsuarioFaker()
    {
        RuleFor(u => u.Id, f => f.Random.Int());
        RuleFor(u => u.Email, f => f.Person.Email);
        RuleFor(u => u.Senha, f => f.Random.String(8));
    }

    public static Usuario GerarUsuarioValido()
    {
        return new UsuarioFaker().Generate();
    }

    public Usuario GerarUsuarioInvalido()
    {
        var usuarioFakerInvalido = this.Clone();
        usuarioFakerInvalido.RuleFor(u => u.Email, f => "invalid-email");

        return usuarioFakerInvalido;
    }

    public static List<Usuario> CriarLista(int quantidade)
    {
        var lista = new List<Usuario>();
        var faker = new UsuarioFaker();

        for (int i = 0; i < quantidade; i++)
            lista.Add(faker.Generate());

        return lista;
    }
}
