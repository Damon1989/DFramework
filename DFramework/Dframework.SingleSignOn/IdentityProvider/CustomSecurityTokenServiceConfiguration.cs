using DFramework.Config;
using System.IdentityModel.Configuration;
using System.Web;
using System.Web.Configuration;
using Dframework.SingleSignOn.Config;

namespace Dframework.SingleSignOn.IdentityProvider
{
    public class CustomSecurityTokenServiceConfiguration : SecurityTokenServiceConfiguration
    {
        private const string CustomSecurityTokenServiceConfigurationKey = "CustomSecurityTokenServiceConfigurationKey";
        private static readonly object SyncRoot = new object();

        public CustomSecurityTokenServiceConfiguration()
        : base(WebConfigurationManager.AppSettings[Common.IssuerName])
        {
            SecurityTokenService = Configuration.Instance.GetCustomSecurityTokenServiceType();
        }

        public static CustomSecurityTokenServiceConfiguration Current
        {
            get
            {
                var app = HttpContext.Current.Application;

                var config =
                    app.Get(CustomSecurityTokenServiceConfigurationKey) as CustomSecurityTokenServiceConfiguration;

                if (config != null)
                {
                    return config;
                }

                lock (SyncRoot)
                {
                    config = app.Get(CustomSecurityTokenServiceConfigurationKey) as CustomSecurityTokenServiceConfiguration;
                    if (config == null)
                    {
                        config = new CustomSecurityTokenServiceConfiguration();
                        app.Add(CustomSecurityTokenServiceConfigurationKey, config);
                    }

                    return config;
                }
            }
        }
    }
}