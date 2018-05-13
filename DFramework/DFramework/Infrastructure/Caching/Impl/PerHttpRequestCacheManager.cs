using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace DFramework.Infrastructure.Caching.Impl
{
    public class PerHttpRequestCacheManager : CacheManagerBase
    {
        private readonly HttpContextBase _context;

        public PerHttpRequestCacheManager(HttpContextBase context)
        {
            _context = context;
        }

        protected virtual IDictionary GetItems()
        {
            return _context?.Items;
        }

        public override CacheValue<T> Get<T>(string key)
        {
            var items = GetItems();
            return items == null ? CacheValue<T>.NoValue : new CacheValue<T>((T)items[key], true);
        }

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

        public override bool IsSet(string key)
        {
            var items = GetItems();
            return items?[key] != null;
        }

        public override void Remove(string key)
        {
            var items = GetItems();

            items?.Remove(key);
        }

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