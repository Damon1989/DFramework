using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using DFramework.Infrastructure;
using DFramework.Infrastructure.Logging;
using log4net.Core;
using log4net.Layout;

namespace IFramework.Log4Net
{
    public class JsonLogLayout : LayoutSkeleton
    {
        private const string AdditionalPropertiesKey = "AdditionalProperties";
        public string App { get; set; }
        public string Module { get; set; }

        public JsonLogLayout()
        {
            IgnoresException = false;
        }

        public override void ActivateOptions()
        {
        }

        public override void Format(TextWriter writer, LoggingEvent loggingEvent)
        {
            var evt = GetJsonObject(loggingEvent);

            var message = evt.ToJson(useCamelCase: true, ignoreNullValue: true);

            writer.Write(message + "\r\n");
        }

        private object GetJsonObject(LoggingEvent loggingEvent)
        {
            var additionalProperties = log4net.LogicalThreadContext
                .Properties[AdditionalPropertiesKey]?
                .ToJson()
                .ToJsonObject<Dictionary<string, object>>();

            var log = loggingEvent.MessageObject as JsonLogBase ?? new JsonLogBase
            {
                Message = loggingEvent.MessageObject
            };
            var stackFrame = loggingEvent.LocationInformation.StackFrames[1];
            log.Method = log.Method ?? stackFrame.Method.Name;
            log.Thread = log.Thread ?? loggingEvent.ThreadName;
            log.Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
            log.App = log.App ?? additionalProperties?.TryGetValue(nameof(log.App), App)?.ToString();
            log.Module = log.Module ?? additionalProperties?.TryGetValue(nameof(log.Module), Module)?.ToString();
            log.Logger = log.Logger ?? additionalProperties?.TryGetValue(nameof(log.Logger), loggingEvent.LoggerName).ToString();
            log.Host = log.Host ?? Environment.MachineName;
            log.Ip = Utility.GetLocalIPV4().ToString();
            log.LogLevel = loggingEvent.Level.ToString();

            if (loggingEvent.ExceptionObject != null)
            {
                log.Exception = new DFramework.Infrastructure.Logging.LogException
                {
                    Class = loggingEvent.ExceptionObject.GetType().ToString(),
                    Message = loggingEvent.ExceptionObject.Message,
                    StackTrace = loggingEvent.ExceptionObject.StackTrace
                };
            }
            var logDict = log.ToJson().ToJsonObject<Dictionary<string, object>>();
            additionalProperties.ForEach(p =>
            {
                if (p.Key != nameof(App) && p.Key != nameof(Module))
                {
                    logDict[p.Key] = p.Value;
                }
            });
            return logDict;
        }
    }
}