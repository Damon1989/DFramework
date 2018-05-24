using DFramework.Infrastructure.Logging;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.Log4Net
{
    public class Log4NetLoggerFactory : ILoggerFactory
    {
        private readonly string _defaultApp;
        private static readonly ConcurrentDictionary<string, ILogger> Loggers = new ConcurrentDictionary<string, ILogger>();

        public Log4NetLoggerFactory(string configFile, string defaultApp, Level defaultLevel = Level.Debug)
        {
            _defaultApp = defaultApp;
            var file = new FileInfo(configFile);
            if (!file.Exists)
            {
                file = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile));
            }

            if (file.Exists)
            {
                XmlConfigurator.ConfigureAndWatch(file);
            }
            else
            {
                BasicConfigurator.Configure(new TraceAppender { Layout = new PatternLayout() });
            }
        }

        public ILogger Create(string name, string app = null, string module = null, Level? level = null, object additionalProperties = null)
        {
            throw new NotImplementedException();
            //if (name == null)
            //{
            //    throw new ArgumentNullException(nameof(name));
            //}
            //app = app ?? _defaultApp;
            //var loggerKey = $"{app}{name}";
            ////var logger = Loggers.GetOrAdd(loggerKey,key=>new Log4NetLogger(LogManager.GetLogger(key),))
        }

        public ILogger Create(Type type, Level? level = null, object addicationalProperties = null)
        {
            throw new NotImplementedException();
        }
    }
}