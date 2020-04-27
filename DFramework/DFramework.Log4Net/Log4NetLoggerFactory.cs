using DFramework.Infrastructure.Logging;
using DFramework.Infrastructure;
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
using DCommon;

namespace DFramework.Log4Net
{
    public class Log4NetLoggerFactory : ILoggerFactory
    {
        private readonly string _defaultApp;
        private readonly ILoggerLevelController _loggerLevelController;
        private static readonly ConcurrentDictionary<string, ILogger> Loggers = new ConcurrentDictionary<string, ILogger>();

        public Log4NetLoggerFactory(string configFile, ILoggerLevelController loggerLevelController, string defaultApp, Level defaultLevel = Level.Debug)
        {
            _defaultApp = defaultApp;
            _loggerLevelController = loggerLevelController;
            _loggerLevelController.SetDefaultLevel(defaultLevel);
            _loggerLevelController.OnLoggerLevelChanged += _loggerLevelController_OnLoggerLevelChanged;
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

        private void _loggerLevelController_OnLoggerLevelChanged(string app, string logger, Level level)
        {
            Loggers.TryGetValue(logger, null)?.ChangeLogLevel(level);
        }

        public ILogger Create(string name, string app = null, string module = null, Level? level = null, object additionalProperties = null)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            app = app ?? _defaultApp;
            var loggerKey = $"{app}{name}";
            var logger = Loggers.GetOrAdd(loggerKey, key => new Log4NetLogger(LogManager.GetLogger(key),
                _loggerLevelController.GetOrAddLoggerLevel(app, name, level),
                app,
                module,
                additionalProperties));
            return logger;
        }

        public ILogger Create(Type type, Level? level = null, object additionalProperties = null)
        {
            return Create(type.FullName, null, null, level, additionalProperties);
        }
    }
}