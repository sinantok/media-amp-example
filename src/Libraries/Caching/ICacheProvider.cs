using System;

namespace Caching
{
    public interface ICacheProvider
    {
        /// <summary>
        /// Defaulte cache timeout
        /// </summary>
        TimeSpan CacheDuration { get; }

        /// <summary>
        /// Retrieve cached item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of cached item</param>
        /// <param name="cacheTime">Cache duration</param>
        /// <param name="acquire">Function to load item if it's not in the cache yet</param>
        /// <returns>Cached value. Default(T) if
        /// item doesn't exist.</returns>
        T Get<T>(string key, TimeSpan cacheTime, Func<T> acquire);

        /// <summary>
        /// Insert value into the cache using
        /// appropriate name/value pairs WITH a cache duration set in seconds
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Item to be cached</param>
        /// <param name="value">Name of item</param>
        /// <param name="cacheTime">Cache duration</param>
        void Set<T>(string key, T value, TimeSpan cacheTime);

        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key">Name of cached item</param>        
        void Clear(string key);
    }
}
