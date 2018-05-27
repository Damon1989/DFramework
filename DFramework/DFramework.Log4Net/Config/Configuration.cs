using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFramework.Config;
using DFramework.Infrastructure.Logging;
using DFramework.IoC;

namespace DFramework.Log4Net.Config
{
    public static class Log4NetConfiguration
    {
        public static Configuration UseLog4Net(this Configuration configuration,
            string defaultApp,
            Level defaultLevel = Level.Debug,
            string configFile = "log4net.config")
        {
            if (string.IsNullOrWhiteSpace(defaultApp))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(defaultApp));
            }
            var loggerLevelController = IoCFactory.Resolve<ILoggerLevelController>();
            IoCFactory.Instance.CurrentContainer.RegisterInstance(typeof(ILoggerFactory),
                                                                  new Log4NetLoggerFactory(configFile, loggerLevelController, defaultApp, defaultLevel));
            return configuration;
        }
    }
}