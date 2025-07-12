namespace Saboro.Core.Interfaces.Services;

public interface IGeminiService
{
    Task<string> ObterDicaDoChefAsync(string nomeReceita);
}
