namespace DCommon
{
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text.RegularExpressions;
    using System.Web;

    public class CommonHelper
    {
        public static IPAddress HostIPv4
        {
            get
            {
                return Dns.GetHostEntry(Dns.GetHostName()).AddressList
                    .First(c => c.AddressFamily == AddressFamily.InterNetwork);
            }
        }

        public static string GetHostAddress()
        {
            var local = "127.0.0.1";
            try
            {
                var userHostAddress = HttpContext.Current?.Request?.UserHostAddress;
                if (string.IsNullOrEmpty(userHostAddress))
                    userHostAddress = HttpContext.Current?.Request?.ServerVariables["REMOTE_ADDR"];

                if (!string.IsNullOrEmpty(userHostAddress) && IsIp(userHostAddress)) return userHostAddress;

                return local;
            }
            catch
            {
                return local;
            }
        }

        public static IPAddress GetLocalIPV4()
        {
            return HostIPv4;
        }

        private static bool IsIp(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
    }
}