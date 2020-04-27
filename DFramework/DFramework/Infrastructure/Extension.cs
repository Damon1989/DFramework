using DCommon;
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

        ///// <summary>
        /////     Adds a char to end of given string if it does not ends with the char.
        ///// </summary>
        ///// <param name="str"></param>
        ///// <param name="c"></param>
        ///// <returns></returns>
        //public static string EnsureEndsWith(this string str, char c)
        //{
        //    return EnsureEndsWith(str, c, StringComparison.Ordinal);
        //}

        ///// <summary>
        /////     Adds a char to end of given string if it does not ends with the char.
        ///// </summary>
        ///// <param name="str"></param>
        ///// <param name="c"></param>
        ///// <param name="comparisionType"></param>
        ///// <returns></returns>
        //public static string EnsureEndsWith(this string str, char c, StringComparison comparisionType)
        //{
        //    if (str == null) throw new ArgumentException(nameof(str));

        //    if (str.EndsWith(c.ToString(), comparisionType)) return str;
        //    return str + c;
        //}

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