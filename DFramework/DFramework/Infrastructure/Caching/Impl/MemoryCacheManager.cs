using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Text.RegularExpressions;

namespace DFramework.Infrastructure.Caching.Impl
{
    /// <summary>
    /// Represents a manager for caching between HTTP requests (long term caching)
    /// </summary>
    public class MemoryCacheManager : CacheManagerBase
    {
        protected ObjectCache Cache => MemoryCache.Default;

        /// <summary>
        ///  Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public override CacheValue<T> Get<T>(string key)
        {
            return Cache[key] as CacheValue<T> ?? CacheValue<T>.NoValue;
        }

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        public override void Set<T>(string key, T data, int cacheTime)
        {
            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime)
            };
            Cache.Add(new CacheItem(key, new CacheValue<T>(data, true)), policy);
        }

        /// <summary>
        ///  Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override bool IsSet(string key)
        {
            return Cache.Contains(key);
        }

        /// <summary>
        ///  Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key"></param>
        public override void Remove(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        ///  Removes items by pattern
        /// </summary>
        /// <param name="pattern"></param>
        public override void RemoveByPattern(string pattern)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = new List<string>();

            foreach (var item in Cache)
            {
                if (regex.IsMatch(item.Key))
                {
                    keysToRemove.Add(item.Key);
                }
            }

            foreach (var key in keysToRemove)
            {
                Remove(key);
            }
        }

        /// <summary>
        ///  Clear all the data
        /// </summary>
        public override void Clear()
        {
            foreach (var item in Cache)
            {
                Remove(item.Key);
            }
        }
    }
}