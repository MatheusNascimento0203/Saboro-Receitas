using Newtonsoft.Json;

namespace Saboro.Core.Extensions;

public static class HttpContentExtensions
{
    public static async Task<T> DeserializeJsonAsync<T>(this HttpContent httpContent, JsonSerializerSettings settings = null)
    {
        var response = await httpContent.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(response, settings);
    }

    public static async Task<string> DeserializeJsonAsStringAsync(this HttpContent httpContent)
    {
        return await httpContent.ReadAsStringAsync();
    }
}