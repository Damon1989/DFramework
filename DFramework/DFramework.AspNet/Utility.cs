using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web;

namespace DFramework.AspNet
{
    internal class Utility
    {
    }

    public static class WebApiUtility
    {
        public const string LocalIPv4 = "127.0.0.1";
        public const string LocalIPv6 = "::1";
        public const string ClientIPNotAllowedMessage = "Client IP is not allowed!";

        public static string GetClientIP(this HttpRequestMessage request)
        {
            if (request != null && request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            if (request != null && request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                var property = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return property != null ? property.Address : null;
            }
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
            return null;
        }
    }
}