namespace DFramework.Infrastructure.Helpers
{
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;

    using DFramework.Infrastructure.Extensions;

    /// <summary>
    ///     字符串操作-工具
    /// </summary>
    public class String
    {
        /// <summary>
        ///     首字母小写
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FirstLowerCase(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                       ? string.Empty
                       : $"{value.Substring(0, 1).ToLower()}{value.Substring(1)}";
        }

        /// <summary>
        ///     首字母大写
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FirstUpperCase(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                       ? string.Empty
                       : $"{value.Substring(0, 1).ToUpper()}{value.Substring(1)}";
        }

        /// <summary>
        ///     将集合连接为带分隔符的字符串
        /// </summary>
        /// <typeparam name="T">集合元素类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="quotes">引号，默认不带引号，范例：单引号"'"</param>
        /// <param name="separator">分隔符，默认使用逗号分隔</param>
        /// <returns></returns>
        public static string Join<T>(IEnumerable<T> list, string quotes = "", string separator = ",")
        {
            if (list == null) return string.Empty;

            var result = new StringBuilder();
            foreach (var each in list) result.Append($"{quotes}{each}{quotes}{separator}");

            return string.IsNullOrEmpty(separator)
                       ? result.ToString()
                       : result.ToString().TrimEnd(separator.ToCharArray());
        }

        /// <summary>
        ///     移除末尾字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="removeValue"></param>
        /// <returns></returns>
        public static string RemoveEnd(string value, string removeValue)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;

            if (string.IsNullOrWhiteSpace(removeValue)) return value.SafeString();

            return value.ToLower().EndsWith(removeValue.ToLower())
                       ? value.Remove(value.Length - removeValue.Length, removeValue.Length)
                       : value;
        }

        /// <summary>
        ///     分隔词组
        /// </summary>
        /// <param name="value"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string SplitWordGroup(string value, char separator = '-')
        {
            var pattern = @"([A-Z])(?=[a-z])|(?<=[a-z])([A-Z]|[0-9]+)";
            return string.IsNullOrWhiteSpace(value)
                       ? string.Empty
                       : Regex.Replace(value, pattern, $"{separator}$1$2").TrimStart(separator).ToLower();
        }
    }
}