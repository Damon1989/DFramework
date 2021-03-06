﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace DFramework.Infrastructure
{
    public static class Utility
    {
        private const string k_base36_digits = "0123456789abcdefghijklmnopqrstuvwxyz";
        private static readonly uint[] _lookup32 = CreateLookup32();

        public static string GetFullNameWithAssembly(this Type type)
        {
            return $"{type.FullName},{type.Assembly.GetName().Name}";
        }

        #region ForEach

        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> act)
        {
            var forEach = source as T[] ?? source.ToArray();
            foreach (var element in forEach.OrEmptyIfNull())
            {
                act(element);
            }
            return forEach;
        }

        #endregion ForEach

        #region IP

        public static IPAddress GetLocalIPV4()
        {
            return HostIPv4;
        }

        /// <summary>
        /// 获取本机IP
        /// </summary>
        public static IPAddress HostIPv4
        {
            get
            {
                return Dns.GetHostEntry(Dns.GetHostName())
                            .AddressList
                            .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            }
        }

        /// <summary>
        /// 获取客户端IP地址(无视代理)
        /// </summary>
        /// <returns>若失败则返回回送地址</returns>
        public static string GetHostAddress()
        {
            try
            {
                var userHostAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
                if (string.IsNullOrEmpty(userHostAddress))
                {
                    userHostAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
                if (!string.IsNullOrEmpty(userHostAddress) && IsIp(userHostAddress))
                {
                    return userHostAddress;
                }
                return "127.0.0.1";
            }
            catch
            {
                return "127.0.0.1";
            }
        }

        private static bool IsIp(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        #endregion IP

        #region FilePath

        public static string GetServerMapPath(string filePath)
        {
            return MapPath(filePath);
        }

        public static string MapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HostingEnvironment.MapPath(strPath);
            }
            else//非web程序引用
            {
                strPath = strPath.Replace("~/", "").Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.TrimStart('\\');
                }
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }

        #endregion FilePath

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

        #region Lambda

        public static LambdaExpression GetLambdaExpression(Type type, string propertyName)
        {
            var param = Expression.Parameter(type);
            Expression body = param;
            foreach (var member in propertyName.Split(','))
            {
                body = Expression.PropertyOrField(body, member);
            }

            return Expression.Lambda(body, param);
        }

        public static LambdaExpression GetLambdaExpression(Type type, Expression expression)
        {
            var propertyName = expression.ToString();
            var index = propertyName.IndexOf('.');
            propertyName = propertyName.Substring(index + 1);
            return GetLambdaExpression(type, propertyName);
        }

        #endregion Lambda

        public static T GetValueByKey<T>(this object obj, string name)
        {
            var retValue = default(T);
            object objValue = null;
            try
            {
                var property = obj.GetType()
                    .GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (property != null)
                {
                    objValue = FastInvoke.GetMethodInvoker(property.GetGetMethod(true))(obj, null);
                }

                if (objValue != null)
                {
                    retValue = (T)objValue;
                }
            }
            catch (Exception)
            {
                retValue = default(T);
            }
            return retValue;
        }

        public static void SetValueByKey(this object obj, string name, object value)
        {
            var property = obj.GetType()
                .GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (property != null)
            {
                FastInvoke.GetMethodInvoker(property.GetSetMethod(true))(obj, new[] { value });
            }
        }

        #region IQueryable
        public static IQueryable<T> GetPageElements<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        {
            return query.Skip(pageIndex * pageSize).Take(pageSize);
        }
        #endregion

        #region  GetQueryString
        /// <summary>
        /// 将查询字符串解析转换为名值集合
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static NameValueCollection GetQueryString(string queryString)
        {
            return queryString.GetQueryString(null);
        }

        /// <summary>
        /// 将查询字符串解析转换为名值集合。
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="encoding"></param>
        /// <param name="isEncoded"></param>
        /// <returns></returns>
        public static NameValueCollection GetQueryString(this string queryString, Encoding encoding, bool isEncoded = true)
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
                        char item = queryString[i];
                        if (item == '=')
                        {
                            if (index < 0)
                            {
                                index = i;
                            }
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
                        value = queryString.Substring(index + 1, (i - index) - 1);
                    }
                    else
                    {
                        key = queryString.Substring(startIndex, i - startIndex);
                    }

                    if (isEncoded)
                    {
                        result[key.UrlDeCode(encoding)] = value.UrlDeCode(encoding);
                    }
                    else
                    {
                        result[key] = value;
                    }

                    if ((i == (count - 1)) && (queryString[i] == '&'))
                    {
                        result[key] = string.Empty;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 解码Url
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
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

        public static string ToBase36string(this byte[] bytes, EndianFormat bytesEndian = EndianFormat.Little,
            bool includeProceedingZeros = true)
        {
            var base36_no_zeros = new RadixEncoding(k_base36_digits, bytesEndian, includeProceedingZeros);
            return base36_no_zeros.Encode(bytes);
        }

        public static string ToHexString(this byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");
            var lookup32 = _lookup32;
            var result = new char[bytes.Length * 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                var val = lookup32[bytes[i]];
                result[2 * i] = (char)val;
                result[2 * i + 1] = (char)(val >> 16);
            }
            return new string(result);
        }

        private static uint[] CreateLookup32()
        {
            var result = new uint[256];
            for (var i = 0; i < 256; i++)
            {
                var s = i.ToString("X2");
                result[i] = s[0] + ((uint)s[1] << 16);
            }
            return result;
        }

        public static T DeepClone<T>(this T obj)
        {
            return obj.ToJson().ToJsonObject<T>();
            //object retval;
            //using (var ms = new MemoryStream())
            //{
            //    var bf = new BinaryFormatter();
            //    //序列化成流
            //    bf.Serialize(ms, obj);
            //    ms.Seek(0, SeekOrigin.Begin);
            //    //反序列化成对象
            //    retval = bf.Deserialize(ms);
            //    ms.Close();
            //}
            //return (T) retval;
        }

        public static TAttribute GetCustomAttribute<TAttribute>(this object obj, bool inherit = true)
        where TAttribute : class
        {
            if (obj is Type)
            {
                var attrs = (obj as Type).GetCustomAttributes(typeof(TAttribute), inherit);
                if (attrs != null)
                {
                    return attrs.FirstOrDefault() as TAttribute;
                }
            }
            else if (obj is FieldInfo)
            {
                var attrs = ((FieldInfo)obj).GetCustomAttributes(typeof(TAttribute), inherit);
                if (attrs != null && attrs.Length > 0)
                {
                    return attrs.FirstOrDefault(attr => attr is TAttribute) as TAttribute;
                }
            }
            else if (obj is PropertyInfo)
            {
                var attrs = ((PropertyInfo)obj).GetCustomAttributes(inherit);
                if (attrs != null && attrs.Length > 0)
                {
                    return attrs.FirstOrDefault(attr => attr is TAttribute) as TAttribute;
                }
            }
            else if (obj is MethodInfo)
            {
                var attrs = (obj as MethodInfo).GetCustomAttributes(inherit);
                if (attrs != null && attrs.Length > 0)
                {
                    return attrs.FirstOrDefault(attr => attrs is TAttribute) as TAttribute;
                }
            }
            else if (obj.GetType().IsDefined(typeof(TAttribute), true))
            {
                var attr = Attribute.GetCustomAttribute(obj.GetType(), typeof(TAttribute), inherit) as TAttribute;
                return attr;
            }

            return null;
        }

        public static object GetValueByKey(this object obj, string name)
        {
            object objValue = null;

            {
                var property = obj.GetType()
                    .GetProperty(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (property != null)
                    objValue = FastInvoke.GetMethodInvoker(property.GetGetMethod(true))(obj, null);

                return objValue;
            }
        }
    }
}