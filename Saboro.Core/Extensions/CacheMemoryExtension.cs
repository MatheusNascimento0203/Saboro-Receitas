using Microsoft.Extensions.Caching.Memory;

namespace Saboro.Core.Extensions;

public static class CacheMemoryExtension
{
    public static void AddToCache<T>(this IMemoryCache cache, string key, T item, TimeSpan expiration)
    {
        cache.Set(key, item, DateTimeOffset.Now.Add(expiration));
    }

    public static T GetFromCache<T>(this IMemoryCache cache, string key)
    {
        cache.TryGetValue(key, out T item);
        return item;
    }

    public static void RemoveFromCache(this IMemoryCache cache, string key)
    {
        cache.Remove(key);
    }
}
