namespace MyMvcTest.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using StackExchange.Redis;

    public class RedisManager
    {
        private static readonly object Locker = new object();

        private static ConnectionMultiplexer _instance;

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
                        _instance = ConnectionMultiplexer.Connect(
                            ConfigurationManager.AppSettings["RedisHost"] ?? "127.0.0.1:6379");
                }

                return _instance;
            }
        }
    }

    public interface IRedisService
    {
        string Get(string key);

        T Get<T>(string key);

        Task<string> GetAsync(string key);

        Task<T> GetAsync<T>(string key);

        Task<T> GetAsync<T>(string key, Func<T> func);

        string HashGet(string key, string property);

        Task<string> HashGetAsync(string key, string property);

        Task<T> HashGetAsync<T>(string key, string property);

        Task<T> HashGetAsync<T>(string key);

        void HashSet<T>(CacheValue<T> cacheValue);

        Task HashSetAsync<T>(CacheValue<T> cacheValue);

        void Remove(string key);

        Task RemoveAsync(string key);

        void Set(CacheValue cacheValue);

        void Set<T>(CacheValue<T> cacheValue);

        void Set<T>(string key, Func<T> func);

        void Set<T>(string key, Func<T> func, int timeout);

        Task SetAsync(CacheValue cacheValue);

        Task SetAsync<T>(CacheValue<T> cacheValue);
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
                this._database = RedisManager.Instance.GetDatabase(db);
            }
        }

        public string Get(string key)
        {
            return this.GetAsync(key).Result;
        }

        public T Get<T>(string key)
        {
            return this.GetAsync<T>(key).Result;
        }

        public async Task<string> GetAsync(string key)
        {
            return await this._database.StringGetAsync(key);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var result = await this._database.StringGetAsync(key);
            if (string.IsNullOrEmpty(result) || string.IsNullOrWhiteSpace(result)) return default;
            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task<T> GetAsync<T>(string key, Func<T> func)
        {
            var result = await this._database.StringGetAsync(key);
            if (string.IsNullOrEmpty(result) || string.IsNullOrWhiteSpace(result))
            {
                this.Set(new CacheValue<T>(key, func.Invoke()));
                return await this.GetAsync(key, func);
            }

            return JsonConvert.DeserializeObject<T>(result);
        }

        public string HashGet(string key, string property)
        {
            return this.HashGetAsync(key, property).Result;
        }

        public async Task<string> HashGetAsync(string key, string property)
        {
            return await this.HashGetAsync<string>(key, property);
        }

        public async Task<T> HashGetAsync<T>(string key, string property)
        {
            var redisValue = await this._database.HashGetAsync(key, property);
            return JsonConvert.DeserializeObject<T>(redisValue);
        }

        public async Task<T> HashGetAsync<T>(string key)
        {
            var hashEntries = await this._database.HashGetAllAsync(key);
            var list = new List<string>();
            hashEntries.ToList().ForEach(
                item => { list.Add($"\"{item.Name.ToString()}\":\"{item.Value.ToString()}\""); });
            var str = "{" + string.Join(",", list) + "}";
            return JsonConvert.DeserializeObject<T>(str);
        }

        public void HashSet<T>(CacheValue<T> cacheValue)
        {
            this.HashSetAsync(cacheValue).Wait();
        }

        public async Task HashSetAsync<T>(CacheValue<T> cacheValue)
        {
            var hashEntryList = new List<HashEntry>();
            var properties = cacheValue.Value.GetType().GetProperties();
            properties.ToList().ForEach(
                propertyInfo =>
                    {
                        var value = propertyInfo.GetValue(cacheValue.Value);
                        hashEntryList.Add(new HashEntry(propertyInfo.Name, JsonConvert.SerializeObject(value)));
                    });
            await this._database.HashSetAsync(cacheValue.Key, hashEntryList.ToArray());
        }

        public void Remove(string key)
        {
            this.RemoveAsync(key).Wait();
        }

        public async Task RemoveAsync(string key)
        {
            await this._database.KeyDeleteAsync(key);
        }

        public void Set(CacheValue cacheValue)
        {
            this.SetAsync(cacheValue).Wait();
        }

        public void Set<T>(CacheValue<T> cacheValue)
        {
            this.SetAsync(cacheValue).Wait();
        }

        public void Set<T>(string key, Func<T> func)
        {
            this.Set(new CacheValue<T>(key, func.Invoke()));
        }

        public void Set<T>(string key, Func<T> func, int timeout)
        {
            this.Set(new CacheValue<T>(key, func.Invoke(), timeout));
        }

        public async Task SetAsync(CacheValue cacheValue)
        {
            await this._database.StringSetAsync(
                cacheValue.Key,
                cacheValue.Value,
                new TimeSpan(0, 0, cacheValue.TimeOut));
        }

        public async Task SetAsync<T>(CacheValue<T> cacheValue)
        {
            await this._database.StringSetAsync(
                cacheValue.Key,
                JsonConvert.SerializeObject(cacheValue.Value),
                new TimeSpan(0, 0, cacheValue.TimeOut));
        }
    }

    public class CacheValue : CacheValue<string>
    {
        public CacheValue()
        {
        }

        public CacheValue(string key, string value)
            : base(key, value)
        {
        }

        public CacheValue(string key, string value, int timeout)
            : base(key, value, timeout)
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
            this.Key = key;
            this.Value = value;
            this.TimeOut = 3600;
        }

        public CacheValue(string key, T value, int timeout)
        {
            this.Key = key;
            this.Value = value;
            this.TimeOut = timeout;
        }

        public string Key { get; set; }

        public int TimeOut { get; set; }

        public T Value { get; set; }
    }
}