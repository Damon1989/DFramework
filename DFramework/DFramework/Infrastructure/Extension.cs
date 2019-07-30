using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;

namespace DFramework.Infrastructure
{
    public static class Extension
    {
        public static string GetDescription(this object obj)
        {
            var fi = obj.GetType().GetField(obj.ToString());
            var arrDesc = (DescriptionAttribute[]) fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return arrDesc.Length > 0 ? arrDesc[0].Description : null;
        }

        public static List<string> SplitStringByFenHao(this string splitStr)
        {
            return splitStr.IsNullOrEmpty() ? new List<string>() : splitStr.Split(';').ToList();
        }

        public static List<T> SplitStringByFenHao<T>(this string splitStr)
        {
            if (splitStr.IsNullOrEmpty()) return Array.Empty<T>().ToList();
            var result = new List<T>();

            var sList = splitStr.Split(';').ToList();
            sList.ForEach(item =>
            {
                if (typeof(T) == typeof(int))
                    result.Add((T) (object) int.Parse(item));
                else
                    result.Add((T) Convert.ChangeType(item, typeof(T)));
            });
            return result;
        }

        public static bool IsNullOrCountZero<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        /// <summary>
        ///     default 0
        /// </summary>
        /// <param name="numStr"></param>
        /// <returns></returns>
        public static int TryParseInt(this string numStr)
        {
            try
            {
                int.TryParse(numStr, out var num);
                return num;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        #region IQueryable

        public static IQueryable<T> GetPageElements<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        {
            return query.Skip(pageIndex * pageSize).Take(pageSize);
        }

        #endregion

        public static TAttribute GetCustomAttribute<TAttribute>(this object obj, bool inherit = true)
            where TAttribute : class
        {
            if (obj is Type)
            {
                var attrs = (obj as Type).GetCustomAttributes(typeof(TAttribute), inherit);
                if (attrs != null) return attrs.FirstOrDefault() as TAttribute;
            }
            else if (obj is FieldInfo)
            {
                var attrs = ((FieldInfo) obj).GetCustomAttributes(typeof(TAttribute), inherit);
                if (attrs != null && attrs.Length > 0)
                    return attrs.FirstOrDefault(attr => attr is TAttribute) as TAttribute;
            }
            else if (obj is PropertyInfo)
            {
                var attrs = ((PropertyInfo) obj).GetCustomAttributes(inherit);
                if (attrs != null && attrs.Length > 0)
                    return attrs.FirstOrDefault(attr => attr is TAttribute) as TAttribute;
            }
            else if (obj is MethodInfo)
            {
                var attrs = (obj as MethodInfo).GetCustomAttributes(inherit);
                if (attrs != null && attrs.Length > 0)
                    return attrs.FirstOrDefault(attr => attrs is TAttribute) as TAttribute;
            }
            else if (obj.GetType().IsDefined(typeof(TAttribute), true))
            {
                var attr = Attribute.GetCustomAttribute(obj.GetType(), typeof(TAttribute), inherit) as TAttribute;
                return attr;
            }

            return null;
        }

        public static string GetFullNameWithAssembly(this Type type)
        {
            return $"{type.FullName},{type.Assembly.GetName().Name}";
        }

        /// <summary>
        ///     Checks and deletes given file if it does exists.
        /// </summary>
        /// <param name="filePath"></param>
        public static void DeleteFileIfExists(this string filePath)
        {
            if (File.Exists(filePath)) File.Delete(filePath);
        }

        public static void CreateDirectoryIfNotExists(this string directory)
        {
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        }

        public static byte[] GetAllBytes(this Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        ///     Adds a char to end of given string if it does not ends with the char.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string EnsureEndsWith(this string str, char c)
        {
            return EnsureEndsWith(str, c, StringComparison.Ordinal);
        }

        /// <summary>
        ///     Adds a char to end of given string if it does not ends with the char.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <param name="comparisionType"></param>
        /// <returns></returns>
        public static string EnsureEndsWith(this string str, char c, StringComparison comparisionType)
        {
            if (str == null) throw new ArgumentException(nameof(str));

            if (str.EndsWith(c.ToString(), comparisionType)) return str;
            return str + c;
        }

        /// <summary>
        ///     Adds a char to end of given string if it dose not ends with the char.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static string EnsureEndsWith(this string str, char c, bool ignoreCase, CultureInfo culture)
        {
            if (str == null) throw new ArgumentException(nameof(str));

            if (str.EndsWith(c.ToString(culture), ignoreCase, culture)) return str;
            return str + c;
        }

        /// <summary>
        ///     Adds a char to beginning of given string if it does not starts with the char.
        /// </summary>
        public static string EnsureStartsWith(this string str, char c)
        {
            return EnsureStartsWith(str, c, StringComparison.Ordinal);
        }

        /// <summary>
        ///     Adds a char to beginning of given string if it does not starts with the char.
        /// </summary>
        public static string EnsureStartsWith(this string str, char c, StringComparison comparisonType)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            if (str.StartsWith(c.ToString(), comparisonType)) return str;

            return c + str;
        }

        /// <summary>
        ///     Adds a char to beginning of given string if it does not starts with the char.
        /// </summary>
        public static string EnsureStartsWith(this string str, char c, bool ignoreCase, CultureInfo culture)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            if (str.StartsWith(c.ToString(culture), ignoreCase, culture)) return str;

            return c + str;
        }

        /// <summary>
        ///     Converts string to enum value.
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">String value to convert</param>
        /// <returns>Returns enum object</returns>
        public static T ToEnum<T>(this string value)
            where T : struct
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            return (T) Enum.Parse(typeof(T), value);
        }

        /// <summary>
        ///     Converts string to enum value.
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">String value to convert</param>
        /// <param name="ignoreCase">Ignore case</param>
        /// <returns>Returns enum object</returns>
        public static T ToEnum<T>(this string value, bool ignoreCase)
            where T : struct
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            return (T) Enum.Parse(typeof(T), value, ignoreCase);
        }

        public static string ToMd5(this string str)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(str);
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                foreach (var hashByte in hashBytes) sb.Append(hashByte.ToString("X2"));

                return sb.ToString();
            }
        }

        #region JoinAsString

        public static string JoinAsString(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source);
        }

        public static string JoinAsString<T>(this IEnumerable<T> source, string separator)
        {
            return string.Join(separator, source);
        }

        #endregion

        #region Add

        public static void Add<T>(this List<T> list, bool condition, T item)
        {
            if (list == null) throw new ArgumentException(nameof(list));
            if (condition) list.Add(item);
        }

        public static bool AddIfNotContains<T>(this ICollection<T> source, T item)
        {
            if (source == null) throw new ArgumentException(nameof(source));

            if (source.Contains(item)) return false;

            source.Add(item);
            return true;
        }

        #endregion

        #region IsNullOrEmpty

        /// <summary>
        ///     Indicates whether this string is null or an System.String.Empty string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrEmpty(this object str)
        {
            return str == null || string.IsNullOrEmpty(str.ToString());
        }

        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return source == null || source.Count <= 0;
        }

        /// <summary>
        ///     indicates whether this string is null,empty,or consists only of white-space characters.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        #endregion

        #region Substring

        /// <summary>
        ///     Gets a substring of a string from beginning of the string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string Left(this string str, int len)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            if (str.Length < len)
                throw new ArgumentException("len argument can not be bigger than given string's length!");

            return str.Substring(0, len);
        }

        /// <summary>
        ///     Gets a substring of a string from end of the string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string Right(this string str, int len)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            if (str.Length < len)
                throw new ArgumentException("len argument can not be bigger than given string's length!");

            return str.Substring(str.Length - len, len);
        }

        #endregion

        #region WhereIf

        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }

        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition,
            Func<T, int, bool> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> queryable, bool condition,
            Expression<Func<T, bool>> predicate)
        {
            return condition ? queryable.Where(predicate) : queryable;
        }

        #endregion

        #region ForEach

        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> act)
        {
            var forEach = source as T[] ?? source.ToArray();
            foreach (var element in forEach.OrEmptyIfNull()) act(element);
            return forEach;
        }

        #endregion ForEach

        #region FileExtension

        public static string GetFileName(this string filePath)
        {
            return Path.GetFileName(filePath);
        }

        public static string GetFileNameWithoutExtension(this string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        public static string GetFileExtension(this string filePath)
        {
            return Path.GetExtension(filePath);
        }

        public static string GetDirectoryName(this string filePath)
        {
            return Path.GetDirectoryName(filePath);
        }

        #endregion

        #region 将查询字符串解析转换为名值集合

        /// <summary>
        ///     将查询字符串解析转换为名值集合
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static NameValueCollection GetQueryString(this string queryString)
        {
            return queryString.GetQueryString(null);
        }

        /// <summary>
        ///     将查询字符串解析转换为名值集合
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="encoding"></param>
        /// <param name="isEncoded"></param>
        /// <returns></returns>
        public static NameValueCollection GetQueryString(this string queryString, Encoding encoding,
            bool isEncoded = true)
        {
            queryString = queryString.Replace("?", "");
            var result = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(queryString))
            {
                var count = queryString.Length;
                for (var i = 0; i < count; i++)
                {
                    var startIndex = i;
                    var index = -1;
                    while (i < count)
                    {
                        var item = queryString[i];
                        if (item == '=')
                        {
                            if (index < 0) index = i;
                        }
                        else if (item == '&')
                        {
                            break;
                        }

                        i++;
                    }

                    string key;
                    var value = string.Empty;
                    if (index >= 0)
                    {
                        key = queryString.Substring(startIndex, index - startIndex);
                        value = queryString.Substring(index + 1, i - index - 1);
                    }
                    else
                    {
                        key = queryString.Substring(startIndex, i - startIndex);
                    }

                    if (isEncoded)
                        result[key.UrlDeCode(encoding)] = value.UrlDeCode(encoding);
                    else
                        result[key] = value;

                    if (i == count - 1 && queryString[i] == '&') result[key] = string.Empty;
                }
            }

            return result;
        }

        public static string UrlDeCode(this string str, Encoding encoding = null)
        {
            if (encoding != null) return HttpUtility.UrlDecode(str, encoding);

            var utf8 = Encoding.UTF8;
            //首先用utf-8进行解码
            var code = HttpUtility.UrlDecode(str.ToUpper(), utf8);
            //将已经解码的字符再次进行编码
            var encode = HttpUtility.UrlEncode(code, utf8).ToUpper();
            encoding = str.ToUpper() == encode ? Encoding.UTF8 : Encoding.GetEncoding("gb2312");
            return HttpUtility.UrlDecode(str, encoding);
        }

        #endregion

        #region IP

        public static IPAddress GetLocalIPV4()
        {
            return HostIPv4;
        }

        /// <summary>
        ///     获取本机IP
        /// </summary>
        public static IPAddress HostIPv4
        {
            get
            {
                return Dns.GetHostEntry(Dns.GetHostName())
                    .AddressList
                    .First(x => x.AddressFamily == AddressFamily.InterNetwork);
            }
        }

        /// <summary>
        ///     获取客户端IP地址(无视代理)
        /// </summary>
        /// <returns>若失败则返回回送地址</returns>
        public static string GetHostAddress()
        {
            try
            {
                var userHostAddress = HttpContext.Current.Request.UserHostAddress;
                if (string.IsNullOrEmpty(userHostAddress))
                    userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
                if (!string.IsNullOrEmpty(userHostAddress) && IsIp(userHostAddress)) return userHostAddress;
                return "127.0.0.1";
            }
            catch
            {
                return "127.0.0.1";
            }
        }

        private static bool IsIp(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        #endregion IP

        #region FilePath

        public static string GetServerMapPath(this string filePath)
        {
            return MapPath(filePath);
        }

        public static string MapPath(string strPath)
        {
            if (HttpContext.Current != null) return HostingEnvironment.MapPath(strPath);

            strPath = strPath.Replace("~/", "").Replace("/", "\\");
            if (strPath.StartsWith("\\")) strPath = strPath.TrimStart('\\');
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
        }

        #endregion FilePath
    }
}