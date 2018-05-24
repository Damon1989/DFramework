using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.Infrastructure.Caching
{
    public static class CacheExtensions
    {
        /// <summary>
        /// Default cache timeout is 60 minutes
        /// </summary>
        /// <summary>
        /// Get a cached item.If it's not in the cache yet,then load and cache it
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="key">Cache key</param>
        /// <param name="acquire">Function to load item if it's not in the cache yet</param>
        /// <returns></returns>
        public static CacheValue<T> Get<T>(this ICacheManager cacheManager, string key, Func<T> acquire)
        {
            return Get(cacheManager, key, 60, acquire);
        }

        /// <summary>
        /// Get a cached item.If it's not in the cache yet,then load and cache it
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="key">Cache key</param>
        /// <param name="cacheTime">Cache time in minutes(0 - do not cache)</param>
        /// <param name="acquire">Function to load item if it's not in the cache yet</param>
        /// <returns>Cached item</returns>
        public static CacheValue<T> Get<T>(this ICacheManager cacheManager, string key, int cacheTime, Func<T> acquire)
        {
            if (cacheTime <= 0)
            {
                return new CacheValue<T>(acquire(), true);
            }

            var cacheValue = cacheManager.Get<T>(key);
            if (!cacheValue.HasValue)
            {
                cacheValue = new CacheValue<T>(acquire(), true);
                cacheManager.Set(key, cacheValue.Value, cacheTime);
            }
            return cacheValue;
        }

        /// <summary>
        ///  Get a cached item.If it's not in the cache yet,then load and cache it
        ///  Default cache timeout is 60 minutes.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="key">Cache key</param>
        /// <param name="func">async function to load item if it's not in the cache yet</param>
        /// <param name="continueOnCapturedContext"></param>
        /// <returns></returns>
        public static Task<CacheValue<T>> GetAsync<T>(this ICacheManager cacheManager,
                                                      string key,
                                                      Func<Task<T>> func,
                                                      bool continueOnCapturedContext = false)
        {
            return cacheManager.GetAsync(key, 60, func, continueOnCapturedContext);
        }

        /// <summary>
        /// Get a cached item.If it's not in the cache yet,then load and cache it
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="key">Cache key</param>
        /// <param name="cacheTime">Cache time in minutes(0 - do not cache)</param>
        /// <param name="func">Async Function to load item if it's not in the cache yet</param>
        /// <param name="continueOnCapturedContext"></param>
        /// <returns>Cached item</returns>
        public static async Task<CacheValue<T>> GetAsync<T>(this ICacheManager cacheManager,
                                                            string key,
                                                            int cacheTime,
                                                            Func<Task<T>> func,
                                                            bool continueOnCapturedContext = false)
        {
            if (cacheTime <= 0)
            {
                return new CacheValue<T>(await func().ConfigureAwait(continueOnCapturedContext), true);
            }

            var cacheValue = await cacheManager.GetAsync<T>(key)
                                           .ConfigureAwait(continueOnCapturedContext);
            if (!cacheValue.HasValue)
            {
                cacheValue = new CacheValue<T>(await func().ConfigureAwait(continueOnCapturedContext), true);
                await cacheManager.SetAsync(key, cacheValue.Value, cacheTime)
                                  .ConfigureAwait(continueOnCapturedContext);

                return cacheValue;
            }
            return cacheValue;
        }

        /// <summary>
        ///  Get a cached item. If it's not in the cache yet, then load and cache it
        ///  Default cache timeout is 60 minutes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheManager"></param>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="continueOnCapturedContext"></param>
        /// <returns></returns>
        public static Task<CacheValue<T>> GetAsync<T>(this ICacheManager cacheManager,
                                            string key,
                                            Func<T> func,
                                            bool continueOnCapturedContext = false)
        {
            return cacheManager.GetAsync(key, 60, func, continueOnCapturedContext);
        }

        /// <summary>
        /// Get a cached item.If it's not in the cache yet,then load and cache it
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="key">Cache key</param>
        /// <param name="cacheTime">Cache time in minutes(0 - do not cache)</param>
        /// <param name="func">Function to load item if it's not in the cache yet</param>
        /// <param name="continueOnCapturedContext"></param>
        /// <returns></returns>
        public static async Task<CacheValue<T>> GetAsync<T>(this ICacheManager cacheManager,
                                               string key,
                                               int cacheTime,
                                               Func<T> func,
                                               bool continueOnCapturedContext = false)
        {
            if (cacheTime <= 0)
            {
                return new CacheValue<T>(func(), true);
            }

            var cacheValue = await cacheManager.GetAsync<T>(key)
                                           .ConfigureAwait(continueOnCapturedContext);
            if (!cacheValue.HasValue)
            {
                cacheValue = new CacheValue<T>(func(), true);
                await cacheManager.SetAsync(key, cacheValue.Value, cacheTime)
                                  .ConfigureAwait(continueOnCapturedContext);

                return cacheValue;
            }
            return cacheValue;
        }
    }
}