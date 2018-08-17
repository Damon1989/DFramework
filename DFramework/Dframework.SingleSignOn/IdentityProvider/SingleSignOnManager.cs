using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Dframework.SingleSignOn.IdentityProvider
{
    public class SingleSignOnManager
    {
        private const string SiteCookieName = "StsSiteCookie";
        private const string SiteName = "StsSite";

        public static string[] SignOut()
        {
            if (HttpContext.Current != null)
            {
                var siteCookie = HttpContext.Current.Request.Cookies[SiteCookieName];

                if (siteCookie != null)
                {
                    return siteCookie.Values.GetValues(SiteName);
                }
            }
            return new string[0];
        }

        public static void RegisterRP(string siteUrl)
        {
            if (HttpContext.Current != null)
            {
                var siteCookie = HttpContext.Current.Request.Cookies[SiteCookieName];
                if (siteCookie == null)
                {
                    siteCookie = new HttpCookie(SiteCookieName);
                }
                siteCookie.Values.Add(SiteName, siteUrl);

                HttpContext.Current.Response.AppendCookie(siteCookie);
            }
        }
    }
}