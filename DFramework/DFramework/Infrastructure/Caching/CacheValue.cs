namespace DFramework.Infrastructure.Caching
{
    public class CacheValue<T>
    {
        public CacheValue(T value, bool hasValue)
        {
            Value = value;
            HasValue = hasValue;
        }

        /// <summary>
        /// 是否存在缓存
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// 判断值是否为空，注意，值为空并不表示没有缓存
        /// </summary>
        public bool IsNull => Value == null;

        public T Value { get; }

        public static CacheValue<T> Null => new CacheValue<T>(default(T), true);
        public static CacheValue<T> NoValue => new CacheValue<T>(default(T), false);
    }
}