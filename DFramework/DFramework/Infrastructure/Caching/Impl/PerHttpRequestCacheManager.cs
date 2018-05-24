using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;

namespace DFramework.Infrastructure.Caching.Impl
{
    public class PerHttpRequestCacheManager : CacheManagerBase
    {
        private readonly HttpContextBase _context;

        /// <summary>
        ///  Ctor
        /// </summary>
        /// <param name="context"></param>
        public PerHttpRequestCacheManager(HttpContextBase context)
        {
            _context = context;
        }

        /// <summary>
        ///  Creates a new instance of the NopRequestCache class
        /// </summary>
        /// <returns></returns>
        protected virtual IDictionary GetItems()
        {
            return _context?.Items;
        }

        /// <summary>
        ///  Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get</param>
        /// <returns>The value associated with the specified key.</returns>
        public override CacheValue<T> Get<T>(string key)
        {
            var items = GetItems();
            return items == null ? CacheValue<T>.NoValue : new CacheValue<T>((T)items[key], true);
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
            var items = GetItems();
            if (items == null)
            {
                return;
            }

            if (items.Contains(key))
            {
                items[key] = data;
            }
            else
            {
                items.Add(key, data);
            }
        }

        /// <summary>
        ///  Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override bool IsSet(string key)
        {
            var items = GetItems();
            return items?[key] != null;
        }

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key"></param>
        public override void Remove(string key)
        {
            var items = GetItems();

            items?.Remove(key);
        }

        /// <summary>
        ///  Removes items by pattern
        /// </summary>
        /// <param name="pattern"></param>
        public override void RemoveByPattern(string pattern)
        {
            var items = GetItems();
            if (items == null)
            {
                return;
            }

            var enumerator = items.GetEnumerator();
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = new List<string>();
            while (enumerator.MoveNext())
            {
                if (regex.IsMatch(enumerator.Key.ToString()))
                {
                    keysToRemove.Add(enumerator.Key.ToString());
                }
            }

            foreach (var key in keysToRemove)
            {
                items.Remove(key);
            }
        }

        /// <summary>
        ///  Clear all cache data
        /// </summary>
        public override void Clear()
        {
            var items = GetItems();
            if (items == null)
            {
                return;
            }

            var enumerator = items.GetEnumerator();
            var keysToRemove = new List<string>();
            while (enumerator.MoveNext())
            {
                if (enumerator.Key != null)
                {
                    keysToRemove.Add(enumerator.Key.ToString());
                }
            }

            foreach (var key in keysToRemove)
            {
                items.Remove(key);
            }
        }
    }
}