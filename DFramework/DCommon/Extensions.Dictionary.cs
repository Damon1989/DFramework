namespace DCommon
{
    using System.Collections.Generic;

    public static partial class Extensions
    {
        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.TryGetValue(key, default);
        }

        public static TValue TryGetValue<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue defaultValue)
        {
            return dictionary.TryGetValue(key, out var result) ? result : defaultValue;
        }
    }
}