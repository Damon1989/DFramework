namespace DCommon
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Hosting;

    public static partial class Extensions
    {
        public static void CreateDirectoryIfNotExists(this string directory)
        {
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        }

        public static void DeleteFileIfExists(this string filePath)
        {
            if (File.Exists(filePath)) File.Delete(filePath);
        }

        public static string EnsureEndsWith(this string str, char c, bool ignoreCase, CultureInfo culture)
        {
            if (str == null) throw new ArgumentException(nameof(str));

            if (str.EndsWith(c.ToString(culture), ignoreCase, culture)) return str;
            return str + c;
        }

        public static string EnsureEndsWith(this string str, char c, StringComparison comparisionType)
        {
            if (str == null) throw new ArgumentException(nameof(str));

            if (str.EndsWith(c.ToString(), comparisionType)) return str;
            return str + c;
        }

        public static string EnsureEndsWith(this string str, char c)
        {
            return EnsureEndsWith(str, c, StringComparison.Ordinal);
        }

        public static string EnsureStartsWith(this string str, char c, bool ignoreCase, CultureInfo culture)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            if (str.StartsWith(c.ToString(culture), ignoreCase, culture)) return str;

            return c + str;
        }

        public static string EnsureStartsWith(this string str, char c, StringComparison comparisonType)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            if (str.StartsWith(c.ToString(), comparisonType)) return str;

            return c + str;
        }

        public static string EnsureStartsWith(this string str, char c)
        {
            return EnsureStartsWith(str, c, StringComparison.Ordinal);
        }

        public static string GetDirectoryName(this string filePath)
        {
            return Path.GetDirectoryName(filePath);
        }

        public static string GetFileExtension(this string filePath)
        {
            return Path.GetExtension(filePath);
        }

        public static string GetFileName(this string filePath)
        {
            return Path.GetFileName(filePath);
        }

        public static string GetFileNameWithoutExtension(this string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        public static NameValueCollection GetQueryString(this string queryString)
        {
            return queryString.GetQueryString(null);
        }

        public static NameValueCollection GetQueryString(
            this string queryString,
            Encoding encoding,
            bool isEncoded = true)
        {
            queryString = queryString.Replace("?", string.Empty);
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
                        result[key.UrlDecode(encoding)] = value.UrlDecode(encoding);
                    else
                        result[key] = value;

                    if (i == count - 1 && queryString[i] == '&') result[key] = string.Empty;
                }
            }

            return result;
        }

        public static string GetServerMapPath(this string filePath)
        {
            return MapPath(filePath);
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
        public static List<T> SplitString<T>(this string splitStr, char separator = ';')
        {
            if (splitStr.IsNullOrEmpty()) return Array.Empty<T>().ToList();
            var result = new List<T>();

            var sList = splitStr.Split(separator).ToList();
            sList.ForEach(item =>
                {
                    if (typeof(T) == typeof(int))
                        result.Add((T)(object)int.Parse(item));
                    else
                        result.Add((T)Convert.ChangeType(item, typeof(T)));
                });
            return result;
        }
        public static string Left(this string str, int len)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            if (str.Length < len)
                throw new ArgumentException("len argument can not be bigger than given string's length!");

            return str.Substring(0, len);
        }

        public static string MapPath(string strPath)
        {
            if (HttpContext.Current != null) return HostingEnvironment.MapPath(strPath);

            strPath = strPath.Replace("~/", string.Empty).Replace("/", "\\");
            if (strPath.StartsWith("\\")) strPath = strPath.TrimStart('\\');
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
        }

        public static string Right(this string str, int len)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            if (str.Length < len)
                throw new ArgumentException("len argument can not be bigger than given string's length!");

            return str.Substring(str.Length - len, len);
        }

        public static T ToEnum<T>(this string value)
            where T : struct
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            return (T)Enum.Parse(typeof(T), value);
        }

        public static T ToEnum<T>(this string value, bool ignoreCase)
            where T : struct
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        public static string UrlDecode(this string str, Encoding encoding = null)
        {
            if (encoding != null) return HttpUtility.UrlDecode(str, encoding);

            var code = HttpUtility.UrlDecode(str.ToUpper(), Encoding.UTF8);
            var encode = HttpUtility.UrlEncode(code, Encoding.UTF8).ToUpper();
            encoding = str.ToUpper() == encode ? Encoding.UTF8 : Encoding.GetEncoding("gb2312");
            return HttpUtility.UrlDecode(str, encoding);
        }


    }
}