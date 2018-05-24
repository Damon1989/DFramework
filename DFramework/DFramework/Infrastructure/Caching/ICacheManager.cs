using System.Threading.Tasks;

namespace DFramework.Infrastructure.Caching
{
    public interface ICacheManager
    {
        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        CacheValue<T> Get<T>(string key);

        Task<CacheValue<T>> GetAsync<T>(string key);

        /// <summary>
        /// Adds the specified key and object to the cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache time</param>
        void Set<T>(string key, T data, int cacheTime);

        Task SetAsync<T>(string key, T data, int cacheTime);

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        bool IsSet(string key);

        Task<bool> IsSetAsync(string key);

        /// <summary>
        /// Removes the value with the specified key form the cache
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);

        Task RemoveAsync(string key);

        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="pattern"></param>
        void RemoveByPattern(string pattern);

        Task RemoveByPatternAsync(string pattern);

        /// <summary>
        /// Clear all cache data
        /// </summary>
        void Clear();

        Task ClearAsync();
    }
}