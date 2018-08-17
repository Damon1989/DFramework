using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFramework.Config;

namespace Dframework.SingleSignOn.Config
{
    public static class DFrameworkConfigurationExtension
    {
        private static Type _customSecurityTokenService;

        public static void SetCustomSecurityTokenServiceType(this Configuration configuration,
            Type customSecurityTokenServiceType)
        {
            _customSecurityTokenService = customSecurityTokenServiceType;
        }

        public static Type GetCustomSecurityTokenServiceType(this Configuration configuration)
        {
            if (_customSecurityTokenService == null)
            {
                throw new NotSupportedException("should call SetCustomSecurityTokenService first!");
            }

            return _customSecurityTokenService;
        }
    }
}