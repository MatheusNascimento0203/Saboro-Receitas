using Saboro.Core.Factories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Saboro.Core.Extensions;

public static class GenericExtensions
{
    public static string ToJson<T>(this T obj, JsonSerializerSettings settings = null) where T : class
    {
        return JsonConvert.SerializeObject(obj, settings ?? JsonFactory.CamelCaseSettings());
    }

    public static T FromJson<T>(this string json, JsonSerializerSettings settings = null)
    {
        return JsonConvert.DeserializeObject<T>(json, settings ?? JsonFactory.CamelCaseSettings());
    }

    public static JToken FromJson(this string json)
    {
        return JToken.Parse(json);
    }
}
