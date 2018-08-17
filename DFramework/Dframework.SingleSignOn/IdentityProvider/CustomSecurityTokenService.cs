using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IdentityModel;
using System.IdentityModel.Configuration;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Newtonsoft.Json;

namespace Dframework.SingleSignOn.IdentityProvider
{
    public abstract class CustomSecurityTokenService : SecurityTokenService
    {
        private readonly EncryptingCredentials _encryptingCredentials;
        private readonly SigningCredentials _signingCredentials;

        protected CustomSecurityTokenService(SecurityTokenServiceConfiguration securityTokenServiceConfiguration)
            : base(securityTokenServiceConfiguration)
        {
            var certificate = CertificateUtil.GetCertificate(StoreName.My, StoreLocation.LocalMachine,
                WebConfigurationManager.AppSettings[Common.SigningCertificateName]);
            _signingCredentials = new X509SigningCredentials(certificate);

            if (!string.IsNullOrWhiteSpace(WebConfigurationManager.AppSettings[Common.EncryptingCertificateName]))
            {
                _encryptingCredentials = new X509EncryptingCredentials(
                    CertificateUtil.GetCertificate(StoreName.My, StoreLocation.LocalMachine
                    , WebConfigurationManager.AppSettings[Common.EncryptingCertificateName]));
            }
        }

        protected abstract ICustomIdentityObject GetCustomIdentity(string identity);

        /// <summary>
        ///     此方法返回要发布的令牌内容。内容由一组ClaimsIdentity实例来表示，每一个实例对应了一个要发布的令牌。当前
        ///     Windows Inentity Foundation只支持单个令牌发布，因此返回的集合必须总是只包含单个实例。
        /// </summary>
        /// <param name="principal">调用方的principal</param>
        /// <param name="request">进入的RST,暂时不用</param>
        /// <param name="scope">由之前通过GetScope方法返回的范围</param>
        /// <returns></returns>
        protected override ClaimsIdentity GetOutputClaimsIdentity(ClaimsPrincipal principal, RequestSecurityToken request, Scope scope)
        {
            //返回一个默认声明集，里面包含自己想要的声明
            //这里你可以通过ClaimsPrincipal来验证用户，并通过它来返回正确的声明。
            var identityName = principal.Identity.Name;
            var identityObject = GetCustomIdentity(identityName);

            var outgoingIdentity = new ClaimsIdentity();
            outgoingIdentity.AddClaim(new Claim(ClaimTypes.Name, identityObject.Name));
            outgoingIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, identityObject.Id));

            outgoingIdentity.AddClaim(new Claim("dframework/Info", JsonConvert.SerializeObject(identityObject)));
            SingleSignOnManager.RegisterRP(scope.AppliesToAddress);

            return outgoingIdentity;
        }

        /// <summary>
        /// 此方法返回用于令牌发布请求的配置。配置由Scope类表示。在这里，我们只发布令牌到一个由encryptingCreds字段表示的EP标识
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override Scope GetScope(ClaimsPrincipal principal, RequestSecurityToken request)
        {
            //使用request的AppliesTo属性和EP标识来创建scope
            var scope = new Scope(request.AppliesTo.Uri.AbsoluteUri, _signingCredentials);

            if (Uri.IsWellFormedUriString(request.ReplyTo, UriKind.Absolute))
            {
                if (request.AppliesTo.Uri.Host != new Uri(request.ReplyTo).Host)
                {
                    scope.ReplyToAddress = request.AppliesTo.Uri.AbsoluteUri;
                }
                else
                {
                    scope.ReplyToAddress = request.ReplyTo;
                }
            }
            else
            {
                Uri resultUri = null;
                if (Uri.TryCreate(request.AppliesTo.Uri, request.ReplyTo, out resultUri))
                {
                    scope.ReplyToAddress = resultUri.AbsoluteUri;
                }
                else
                {
                    scope.ReplyToAddress = request.AppliesTo.Uri.ToString();
                }
            }

            if (_encryptingCredentials != null)
            {
                scope.EncryptingCredentials = _encryptingCredentials;
            }
            else
            {
                scope.TokenEncryptionRequired = false;
            }

            return scope;
        }

        public static string ConstructQueryString(NameValueCollection parameters)
        {
            var result = new List<string>();

            foreach (string item in parameters)
            {
                result.Add(string.Concat(item, "=", HttpUtility.UrlEncode(parameters[item])));
            }

            return string.Join("&", result.ToArray());
        }

        public static string SetQueryString(string url, string key, string value)
        {
            if (url.IndexOf("?") > -1)
            {
                var host = url.Split('?')[0];
                var queryString = HttpUtility.ParseQueryString(url.Split('?')[1]);
                queryString[key] = value;
                return $"{host}?{ConstructQueryString(queryString)}";
            }

            return $"{url}?{key}={value}";
        }
    }
}