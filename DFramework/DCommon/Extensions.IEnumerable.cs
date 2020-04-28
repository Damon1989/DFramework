namespace DCommon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static partial class Extensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> act)
        {
            var forEach = source as T[] ?? source.ToArray();
            foreach (var element in forEach.OrEmptyIfNull()) act(element);

            return forEach;
        }

        public static bool IsNullOrCountZero<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        /// <summary>
        ///     将集合连接为带分隔符的字符串
        /// </summary>
        /// <typeparam name="T">集合元素类型</typeparam>
        /// <param name="source">集合</param>
        /// <param name="quotes">引号，默认不带引号，范例：单引号 "'"</param>
        /// <param name="separator">分隔符，默认使用逗号分隔</param>
        public static string JoinAsString<T>(this IEnumerable<T> source, string quotes = "", string separator = ",")
        {
            if (source == null)
                return string.Empty;
            var result = new StringBuilder();
            foreach (var each in source)
                result.AppendFormat("{0}{1}{0}{2}", quotes, each, separator);
            return separator == string.Empty ? result.ToString() : result.ToString().TrimEnd(separator.ToCharArray());
        }

        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }

        public static IEnumerable<T> WhereIf<T>(
            this IEnumerable<T> source,
            bool condition,
            Func<T, int, bool> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }
    }
}