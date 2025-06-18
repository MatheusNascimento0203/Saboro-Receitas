using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Saboro.Core.Factories;

public static class JsonFactory
{
    public static JsonSerializerSettings DefaultSettings(bool ignoreSerializableAttribute = true)
    {
        return new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new DefaultContractResolver
            {
                IgnoreSerializableAttribute = ignoreSerializableAttribute
            }
        };
    }

    public static JsonSerializerSettings CamelCaseSettings()
    {
        return new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }
}
