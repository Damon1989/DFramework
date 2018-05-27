using System.Collections.Generic;

namespace DFramework.Infrastructure
{
    public static class DictionaryExtensions
    {
        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.TryGetValue(key, default(TValue));
        }

        /// <summary>
        /// Gets an item from the dictionary,if it's found.Otherwise,
        /// returns the specified default value.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
                                                       TKey key,
                                                       TValue defaultValue)
        {
            var result = defaultValue;
            if (!dictionary.TryGetValue(key, out result))
            {
                return defaultValue;
            }
            return result;
        }
    }
}