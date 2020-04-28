namespace DFramework.Infrastructure
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Hosting;

    public static class Extension
    {
        /// <summary>
        ///     获取本机IP
        /// </summary>
        public static IPAddress HostIPv4
        {
            get
            {
                return Dns.GetHostEntry(Dns.GetHostName()).AddressList
                    .First(x => x.AddressFamily == AddressFamily.InterNetwork);
            }
        }

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
                var attrs = ((FieldInfo)obj).GetCustomAttributes(typeof(TAttribute), inherit);
                if (attrs != null && attrs.Length > 0)
                    return attrs.FirstOrDefault(attr => attr is TAttribute) as TAttribute;
            }
            else if (obj is PropertyInfo)
            {
                var attrs = ((PropertyInfo)obj).GetCustomAttributes(inherit);
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

        public static string GetDescription(this object obj)
        {
            var fi = obj.GetType().GetField(obj.ToString());
            var arrDesc = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return arrDesc.Length > 0 ? arrDesc[0].Description : null;
        }

        public static string GetFullNameWithAssembly(this Type type)
        {
            return $"{type.FullName},{type.Assembly.GetName().Name}";
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

                // 最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
                if (!string.IsNullOrEmpty(userHostAddress) && IsIp(userHostAddress)) return userHostAddress;
                return "127.0.0.1";
            }
            catch
            {
                return "127.0.0.1";
            }
        }

        public static IPAddress GetLocalIPV4()
        {
            return HostIPv4;
        }

        public static IQueryable<T> GetPageElements<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        {
            return query.Skip(pageIndex * pageSize).Take(pageSize);
        }

        public static string GetServerMapPath(this string filePath)
        {
            return MapPath(filePath);
        }

        public static string MapPath(string strPath)
        {
            if (HttpContext.Current != null) return HostingEnvironment.MapPath(strPath);

            strPath = strPath.Replace("~/", string.Empty).Replace("/", "\\");
            if (strPath.StartsWith("\\")) strPath = strPath.TrimStart('\\');
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
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

        private static bool IsIp(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
    }
}