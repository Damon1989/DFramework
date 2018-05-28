using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Hosting;

namespace DFramework.Infrastructure
{
    public static class Utility
    {
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
    }
}