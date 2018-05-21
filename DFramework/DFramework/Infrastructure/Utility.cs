using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.Infrastructure
{
    public static class Utility
    {
        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> act)
        {
            foreach (var element in source.OrEmptyIfNull())
            {
                act(element);
            }
            return source;
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
    }
}