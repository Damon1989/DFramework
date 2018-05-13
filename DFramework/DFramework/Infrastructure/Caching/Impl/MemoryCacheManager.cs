using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DFramework.Infrastructure.Caching.Impl
{
    /// <summary>
    /// Represents a manager for caching between HTTP requests (long term caching)
    /// </summary>
    public class MemoryCacheManager : CacheManagerBase
    {
        protected ObjectCache Cache => MemoryCache.Default;

        public override CacheValue<T> Get<T>(string key)
        {
            return Cache[key] as CacheValue<T> ?? CacheValue<T>.NoValue;
        }

        public override void Set<T>(string key, T data, int cacheTime)
        {
            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime)
            };
            Cache.Add(new CacheItem(key, new CacheValue<T>(data, true)), policy);
        }

        public override bool IsSet(string key)
        {
            return Cache.Contains(key);
        }

        public override void Remove(string key)
        {
            Cache.Remove(key);
        }

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

        public override void Clear()
        {
            foreach (var item in Cache)
            {
                Remove(item.Key);
            }
        }
    }
}