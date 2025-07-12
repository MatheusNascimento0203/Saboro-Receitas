using Microsoft.Extensions.Options;
using Saboro.Core.Interfaces.Services;
using Saboro.Core.Settings;
using System.Net.Http.Json;
using System.Text.Json;

namespace Saboro.Web.Services;

public class GeminiService : IGeminiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GeminiService(HttpClient httpClient, IOptions<GeminiSettings> geminiSettings)
    {
        _httpClient = httpClient;
        _apiKey = geminiSettings.Value.ApiKey;
    }

    public async Task<string> ObterDicaDoChefAsync(string nomeReceita)
    {
        var prompt = $"Dê uma dica de chef curta e útil para a receita '{nomeReceita}'. Seja criativo e profissional.";

        var request = new
        {
            contents = new[]
            {
                new {
                    parts = new[] { new { text = prompt } }
                }
            }
        };

        var requestBody = JsonContent.Create(request);

        for (int tentativa = 0; tentativa < 3; tentativa++)
        {
            var response = await _httpClient.PostAsync(
                $"https://generativelanguage.googleapis.com/v1/models/gemini-1.5-flash:generateContent?key={_apiKey}",
                requestBody
            );

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<GeminiResponse>();
                return result?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text ?? "Não foi possível gerar uma dica.";
            }

            if ((int)response.StatusCode == 503)
                await Task.Delay(2000 + (tentativa * 1000)); // 2s, 3s, 4s

            else
                break;
        }

        return "Serviço indisponível. Tente novamente em instantes.";
    }

    public class GeminiResponse
    {
        public List<Candidate> Candidates { get; set; }

        public class Candidate
        {
            public Content Content { get; set; }
        }

        public class Content
        {
            public List<Part> Parts { get; set; }
        }

        public class Part
        {
            public string Text { get; set; }
        }
    }
}
