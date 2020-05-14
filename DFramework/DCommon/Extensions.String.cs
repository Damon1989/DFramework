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
        /// <summary>
        ///     Create Directory If Not Exists
        /// </summary>
        /// <param name="directory"></param>
        public static void CreateDirectoryIfNotExists(this string directory)
        {
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        }

        /// <summary>
        ///     Delete File If Exists
        /// </summary>
        /// <param name="filePath"></param>
        public static void DeleteFileIfExists(this string filePath)
        {
            if (File.Exists(filePath)) File.Delete(filePath);
        }

        /// <summary>
        ///     Adds a char to end of given string if it does not ends with the char.
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
        ///     Adds a char to beginning of given string if it does not starts with the char.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string EnsureEndsWith(this string str, char c)
        {
            return EnsureEndsWith(str, c, StringComparison.Ordinal);
        }

        /// <summary>
        ///     Adds a char to beginning of given string if it does not starts with the char.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static string EnsureStartsWith(this string str, char c, bool ignoreCase, CultureInfo culture)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            if (str.StartsWith(c.ToString(culture), ignoreCase, culture)) return str;

            return c + str;
        }

        /// <summary>
        ///     Adds a char to beginning of given string if it does not starts with the char.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        public static string EnsureStartsWith(this string str, char c, StringComparison comparisonType)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            if (str.StartsWith(c.ToString(), comparisonType)) return str;

            return c + str;
        }

        /// <summary>
        ///     Adds a char to beginning of given string if it does not starts with the char.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <returns></returns>
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

        public static List<T> SplitString<T>(this string splitStr, char separator = ';')
        {
            if (splitStr.IsNullOrEmpty()) return Array.Empty<T>().ToList();
            var result = new List<T>();

            var sList = splitStr.Split(separator).ToList();
            sList.ForEach(
                item =>
                    {
                        if (typeof(T) == typeof(int))
                            result.Add((T)(object)int.Parse(item));
                        else
                            result.Add((T)Convert.ChangeType(item, typeof(T)));
                    });
            return result;
        }

        /// <summary>
        ///     转换为bool
        /// </summary>
        /// <param name="obj">数据</param>
        public static bool ToBool(this string obj)
        {
            return RawHelper.Convert.ToBool(obj);
        }

        /// <summary>
        ///     转换为可空bool
        /// </summary>
        /// <param name="obj">数据</param>
        public static bool? ToBoolOrNull(this string obj)
        {
            return RawHelper.Convert.ToBoolOrNull(obj);
        }

        /// <summary>
        ///     转换为日期
        /// </summary>
        /// <param name="obj">数据</param>
        public static DateTime ToDate(this string obj)
        {
            return RawHelper.Convert.ToDate(obj);
        }

        /// <summary>
        ///     转换为可空日期
        /// </summary>
        /// <param name="obj">数据</param>
        public static DateTime? ToDateOrNull(this string obj)
        {
            return RawHelper.Convert.ToDateOrNull(obj);
        }

        /// <summary>
        ///     转换为decimal
        /// </summary>
        /// <param name="obj">数据</param>
        public static decimal ToDecimal(this string obj)
        {
            return RawHelper.Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     转换为可空decimal
        /// </summary>
        /// <param name="obj">数据</param>
        public static decimal? ToDecimalOrNull(this string obj)
        {
            return RawHelper.Convert.ToDecimalOrNull(obj);
        }

        /// <summary>
        ///     转换为double
        /// </summary>
        /// <param name="obj">数据</param>
        public static double ToDouble(this string obj)
        {
            return RawHelper.Convert.ToDouble(obj);
        }

        /// <summary>
        ///     转换为可空double
        /// </summary>
        /// <param name="obj">数据</param>
        public static double? ToDoubleOrNull(this string obj)
        {
            return RawHelper.Convert.ToDoubleOrNull(obj);
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

        /// <summary>
        ///     转换为Guid
        /// </summary>
        /// <param name="obj">数据</param>
        public static Guid ToGuid(this string obj)
        {
            return RawHelper.Convert.ToGuid(obj);
        }

        /// <summary>
        ///     转换为Guid集合
        /// </summary>
        /// <param name="obj">数据,范例: "83B0233C-A24F-49FD-8083-1337209EBC9A,EAB523C6-2FE7-47BE-89D5-C6D440C3033A"</param>
        public static List<Guid> ToGuidList(this string obj)
        {
            return RawHelper.Convert.ToGuidList(obj);
        }

        /// <summary>
        ///     转换为Guid集合
        /// </summary>
        /// <param name="obj">字符串集合</param>
        public static List<Guid> ToGuidList(this IList<string> obj)
        {
            if (obj == null)
                return new List<Guid>();
            return obj.Select(t => t.ToGuid()).ToList();
        }

        /// <summary>
        ///     转换为可空Guid
        /// </summary>
        /// <param name="obj">数据</param>
        public static Guid? ToGuidOrNull(this string obj)
        {
            return RawHelper.Convert.ToGuidOrNull(obj);
        }

        /// <summary>
        ///     转换为int
        /// </summary>
        /// <param name="obj">数据</param>
        public static int ToInt(this string obj)
        {
            return RawHelper.Convert.ToInt(obj);
        }

        /// <summary>
        ///     转换为可空int
        /// </summary>
        /// <param name="obj">数据</param>
        public static int? ToIntOrNull(this string obj)
        {
            return RawHelper.Convert.ToIntOrNull(obj);
        }

        /// <summary>
        ///     转换为long
        /// </summary>
        /// <param name="obj">数据</param>
        public static long ToLong(this string obj)
        {
            return RawHelper.Convert.ToLong(obj);
        }

        /// <summary>
        ///     转换为可空long
        /// </summary>
        /// <param name="obj">数据</param>
        public static long? ToLongOrNull(this string obj)
        {
            return RawHelper.Convert.ToLongOrNull(obj);
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