using System;
using System.IdentityModel.Services;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;

namespace Dframework.SingleSignOn
{
    public class SingleSignOnContext<TUser> where TUser : class
    {
        public static TUser CurrentUser
        {
            get
            {
                try
                {
                    var user = HttpContext.Current.Items["ssouser"] as TUser;
                    if (user == null && HttpContext.Current.User != null)
                    {
                        var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
                        if (claimsIdentity != null)
                        {
                            var claim = claimsIdentity.FindFirst("dframework/Info");
                            if (claim != null)
                            {
                                var userInfo = claim.Value;
                                user = JsonConvert.DeserializeObject<TUser>(userInfo);
                                HttpContext.Current.Items["ssouser"] = user;
                            }
                        }
                    }

                    return user;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public static SignInRequestMessage SignOut(string signOutUrl)
        {
            var fam = FederatedAuthentication.WSFederationAuthenticationModule;
            try
            {
                FormsAuthentication.SignOut();
            }
            finally
            {
                fam.SignOut(true);
            }

            var currentAudienceUri = GetCurrentAudienceUri();
            return new SignInRequestMessage(new Uri(new Uri(fam.Issuer), signOutUrl), currentAudienceUri.AbsoluteUri);
        }

        public static Uri GetCurrentAudienceUri()
        {
            var audienceUris = FederatedAuthentication.WSFederationAuthenticationModule.FederationConfiguration
                .IdentityConfiguration.AudienceRestriction.AllowedAudienceUris;
            var currentUrl = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
            var compareUrl = currentUrl.EndsWith("/") ? currentUrl : currentUrl + "/";
            var curentAudienceUri =
                audienceUris.FirstOrDefault(c => compareUrl.ToLower().Contains(c.AbsoluteUri.ToLower()));
            if (curentAudienceUri == null)
            {
                throw new Exception("未找到与访问地址匹配的audienceUri,请检查配置文件中的audienceUris");
            }

            return curentAudienceUri;
        }
    }
}