using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MyMvcTest.Helper
{
    public class RedisManager
    {
        private static ConnectionMultiplexer _instance;
        private static readonly object Locker = new object();

        private RedisManager()
        {
        }

        public static ConnectionMultiplexer Instance
        {
            get
            {
                if (_instance != null) return _instance;
                var locker = Locker;
                lock (locker)
                {
                    if (_instance == null)
                        _instance = ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["RedisHost"],
                            null);
                }

                return _instance;
            }
        }
    }

    public interface IRedisService
    {
        void Set(CacheValue cacheValue);
        Task SetAsync(CacheValue cacheValue);
        void Set<T>(CacheValue<T> cacheValue);
        Task SetAsync<T>(CacheValue<T> cacheValue);
        void Set<T>(string key, Func<T> func);
        void Set<T>(string key, Func<T> func, int timeout);
        void HashSet<T>(CacheValue<T> cacheValue);
        Task HashSetAsync<T>(CacheValue<T> cacheValue);
        string Get(string key);
        Task<string> GetAsync(string key);
        T Get<T>(string key);
        Task<T> GetAsync<T>(string key);
        string HashGet(string key, string property);
        Task<string> HashGetAsync(string key, string property);
        Task<T> HashGetAsync<T>(string key, string property);
        Task<T> HashGetAsync<T>(string key);
        void Remove(string key);
        Task RemoveAsync(string key);
    }

    public class RedisService : IRedisService
    {
        private readonly IDatabase _database;

        public RedisService()
        {
            var db = 0;
            try
            {
                db = int.Parse(ConfigurationManager.AppSettings["db"]);
            }
            catch (Exception e)
            {
            }
            finally
            {
                _database = RedisManager.Instance.GetDatabase(db);
            }
        }

        public void Set(CacheValue cacheValue)
        {
            SetAsync(cacheValue).Wait();
        }

        public async Task SetAsync(CacheValue cacheValue)
        {
            await _database.StringSetAsync(cacheValue.Key, cacheValue.Value, new TimeSpan(0, 0, cacheValue.TimeOut));
        }

        public void Set<T>(CacheValue<T> cacheValue)
        {
            SetAsync(cacheValue).Wait();
        }

        public async Task SetAsync<T>(CacheValue<T> cacheValue)
        {
            await _database.StringSetAsync(cacheValue.Key, JsonConvert.SerializeObject(cacheValue.Value),
                new TimeSpan(0, 0, cacheValue.TimeOut));
        }

        public void Set<T>(string key, Func<T> func)
        {
            Set(new CacheValue<T>(key, func.Invoke()));
        }

        public void Set<T>(string key, Func<T> func, int timeout)
        {
            Set(new CacheValue<T>(key, func.Invoke(), timeout));
        }

        public void HashSet<T>(CacheValue<T> cacheValue)
        {
            HashSetAsync(cacheValue).Wait();
        }

        public async Task HashSetAsync<T>(CacheValue<T> cacheValue)
        {
            var hashEntryList = new List<HashEntry>();
            var properties = cacheValue.Value.GetType().GetProperties();
            properties.ToList().ForEach(propertyInfo =>
            {
                var value = propertyInfo.GetValue(cacheValue.Value);
                hashEntryList.Add(new HashEntry(propertyInfo.Name, JsonConvert.SerializeObject(value)));
            });
            await _database.HashSetAsync(cacheValue.Key, hashEntryList.ToArray());
        }

        public string Get(string key)
        {
            return GetAsync(key).Result;
        }

        public T Get<T>(string key)
        {
            return GetAsync<T>(key).Result;
        }

        public async Task<string> GetAsync(string key)
        {
            return await _database.StringGetAsync(key);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var result = await _database.StringGetAsync(key);
            if (string.IsNullOrEmpty(result) || string.IsNullOrWhiteSpace(result)) return default(T);
            return JsonConvert.DeserializeObject<T>(result);
        }

        public string HashGet(string key, string property)
        {
            return HashGetAsync(key, property).Result;
        }

        public async Task<string> HashGetAsync(string key, string property)
        {
            return await HashGetAsync<string>(key, property);
        }

        public async Task<T> HashGetAsync<T>(string key, string property)
        {
            var redisValue = await _database.HashGetAsync(key, property);
            return JsonConvert.DeserializeObject<T>(redisValue);
        }

        public async Task<T> HashGetAsync<T>(string key)
        {
            var hashEntries = await _database.HashGetAllAsync(key);
            var list = new List<string>();
            hashEntries.ToList().ForEach(item =>
            {
                list.Add($"\"{item.Name.ToString()}\":\"{item.Value.ToString()}\"");
            });
            var str = "{" + string.Join(",", list) + "}";
            return JsonConvert.DeserializeObject<T>(str);
        }

        public void Remove(string key)
        {
            RemoveAsync(key).Wait();
        }

        public async Task RemoveAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }
    }

    public class CacheValue : CacheValue<string>
    {
        public CacheValue()
        {
        }

        public CacheValue(string key, string value) : base(key, value)
        {
        }

        public CacheValue(string key, string value, int timeout) : base(key, value, timeout)
        {
        }
    }

    public class CacheValue<T>
    {
        public CacheValue()
        {
        }

        public CacheValue(string key, T value)
        {
            Key = key;
            Value = value;
            TimeOut = 3600;
        }

        public CacheValue(string key, T value, int timeout)
        {
            Key = key;
            Value = value;
            TimeOut = timeout;
        }

        public string Key { get; set; }


        public T Value { get; set; }

        public int TimeOut { get; set; }
    }
}