using DFramework.Infrastructure;
using log4net;
using log4net.Core;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using ILogger = DFramework.Infrastructure.Logging.ILogger;
using Level = log4net.Core.Level;

namespace DFramework.Log4Net
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _log;
        public string App { get; protected set; }
        public string Module { get; protected set; }
        public Dictionary<string, object> AdditionalProperties { get; set; }

        public Infrastructure.Logging.Level Level => ToLoggingLevel(((Logger)_log.Logger).Level);
        public string Name => ((Logger)_log.Logger).Name;

        public Log4NetLogger(ILog log,
                            Infrastructure.Logging.Level level = Infrastructure.Logging.Level.Debug,
                            string app = null,
                            string module = null,
                            object additionalProperties = null)
        {
            _log = log;
            ChangeLogLevel(level);
            App = app;
            Module = module;
            SetAdditionalProperties(additionalProperties);
        }

        public static Level ToLog4NetLevel(Infrastructure.Logging.Level level)
        {
            var log4NetLevel = log4net.Core.Level.Debug;
            switch (level)
            {
                case Infrastructure.Logging.Level.All:
                    log4NetLevel = log4net.Core.Level.All;
                    break;

                case Infrastructure.Logging.Level.Debug:
                    log4NetLevel = log4net.Core.Level.Debug;
                    break;

                case Infrastructure.Logging.Level.Info:
                    log4NetLevel = log4net.Core.Level.Info;
                    break;

                case Infrastructure.Logging.Level.Warn:
                    log4NetLevel = log4net.Core.Level.Warn;
                    break;

                case Infrastructure.Logging.Level.Error:
                    log4NetLevel = log4net.Core.Level.Error;
                    break;

                case Infrastructure.Logging.Level.Fatal:
                    log4NetLevel = log4net.Core.Level.Fatal;
                    break;
            }
            return log4NetLevel;
        }

        public static Infrastructure.Logging.Level ToLoggingLevel(Level level)
        {
            var loggingLevel = Infrastructure.Logging.Level.Debug;
            if (level == log4net.Core.Level.All)
            {
                loggingLevel = Infrastructure.Logging.Level.All;
            }
            else if (level == log4net.Core.Level.Debug)
            {
                loggingLevel = Infrastructure.Logging.Level.Debug;
            }
            else if (level == log4net.Core.Level.Info)
            {
                loggingLevel = Infrastructure.Logging.Level.Info;
            }
            else if (level == log4net.Core.Level.Warn)
            {
                loggingLevel = Infrastructure.Logging.Level.Warn;
            }
            else if (level == log4net.Core.Level.Error)
            {
                loggingLevel = Infrastructure.Logging.Level.Error;
            }
            else if (level == log4net.Core.Level.Fatal)
            {
                loggingLevel = Infrastructure.Logging.Level.Fatal;
            }

            return loggingLevel;
        }

        protected void SetAdditionalProperties(object additionalProperties)
        {
            var addicationDict = AdditionalProperties?.ToJson().ToJsonObject<Dictionary<string, object>>() ??
                new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(App))
            {
                addicationDict[nameof(App)] = App;
            }
            if (!string.IsNullOrWhiteSpace(Module))
            {
                addicationDict[nameof(Module)] = Module;
            }
            if (Name.StartsWith(App))
            {
                addicationDict["Logger"] = Name.Substring(App.Length);
            }
            AdditionalProperties = addicationDict;
        }

        private void SetAdditionalProperties()
        {
            LogicalThreadContext.Properties[nameof(AdditionalProperties)] = AdditionalProperties;
        }

        public void ChangeLogLevel(Infrastructure.Logging.Level level)
        {
            ((Logger)_log.Logger).Level = ToLog4NetLevel(level);
        }

        public void Debug(object message)
        {
            SetAdditionalProperties();
            _log.Debug(message);
        }

        public void Debug(object message, Exception exception)
        {
            SetAdditionalProperties();
            _log.Debug(message, exception);
        }

        public void DebugFormat(string format, params object[] args)
        {
            SetAdditionalProperties();
            _log.DebugFormat(format, args);
        }

        public void Error(object message)
        {
            SetAdditionalProperties();
            _log.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            SetAdditionalProperties();
            _log.Error(message, exception);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            SetAdditionalProperties();
            _log.ErrorFormat(format, args);
        }

        public void Fatal(object message)
        {
            SetAdditionalProperties();
            _log.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            SetAdditionalProperties();
            _log.Fatal(message, exception);
        }

        public void FatalFormat(string format, params object[] args)
        {
            SetAdditionalProperties();
            _log.FatalFormat(format, args);
        }

        public void Info(object message)
        {
            SetAdditionalProperties();
            _log.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            SetAdditionalProperties();
            _log.Info(message, exception);
        }

        public void InfoFormat(string format, params object[] args)
        {
            SetAdditionalProperties();
            _log.InfoFormat(format, args);
        }

        public void Warn(object message)
        {
            SetAdditionalProperties();
            _log.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            SetAdditionalProperties();
            _log.Warn(message, exception);
        }

        public void WarnFormat(string format, params object[] args)
        {
            SetAdditionalProperties();
            _log.WarnFormat(format, args);
        }
    }
}