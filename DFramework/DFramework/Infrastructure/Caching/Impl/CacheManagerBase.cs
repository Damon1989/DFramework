using System.Threading.Tasks;

namespace DFramework.Infrastructure.Caching.Impl
{
    public abstract class CacheManagerBase : ICacheManager
    {
        public abstract CacheValue<T> Get<T>(string key);

        public virtual Task<CacheValue<T>> GetAsync<T>(string key)
        {
            return Task.FromResult(Get<T>(key));
        }

        public abstract void Set<T>(string key, T data, int cacheTime);

        public virtual Task SetAsync<T>(string key, T data, int cacheTime)
        {
            return Task.Run(() => Set(key, data, cacheTime));
        }

        public abstract bool IsSet(string key);

        public virtual Task<bool> IsSetAsync(string key)
        {
            return Task.FromResult(IsSet(key));
        }

        public abstract void Remove(string key);

        public virtual Task RemoveAsync(string key)
        {
            return Task.Run(() => Remove(key));
        }

        public abstract void RemoveByPattern(string pattern);

        public virtual Task RemoveByPatternAsync(string pattern)
        {
            return Task.Run(() => RemoveByPattern(pattern));
        }

        public abstract void Clear();

        public virtual Task ClearAsync()
        {
            return Task.Run(() => Clear());
        }
    }
}