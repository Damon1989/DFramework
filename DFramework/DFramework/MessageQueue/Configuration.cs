using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFramework.Command;
using DFramework.Command.Impl;
using DFramework.Config;
using DFramework.Event;
using DFramework.Event.Impl;
using DFramework.IoC;
using DFramework.Message;
using DFramework.Message.Impl;

namespace DFramework.MessageQueue
{
    public static class DFrameworkConfigurationExtension
    {
        private static string _messageQueueNameFormat = string.Empty;
        private static string _appNameFormat = string.Empty;
        private static TimeSpan _receiveMessageTimeout = new TimeSpan(0, 0, 10);

        private static string _defaultTopic = string.Empty;

        public static string AppName { get; private set; }

        public static Configuration SetDefaultTopic(this Configuration configuration, string defaultTopic)
        {
            _defaultTopic = defaultTopic;
            return configuration;
        }

        public static string GetDefaultTopic(this Configuration configuration)
        {
            return _defaultTopic;
        }

        public static Configuration UseMessageQueue(this Configuration configuration, string appName = null)
        {
            AppName = appName;
            var appNameFormat = string.IsNullOrEmpty(appName) ? "{0}" : AppName + ".{0}";
            configuration.SetAppNameFormat(appNameFormat).UseMockCommandBus().UseMockMessagePublisher();
            return configuration;
        }

        public static Configuration UseDefaultEventBus(this Configuration configuration)
        {
            IoCFactory.Instance.CurrentContainer.RegisterType<IEventBus, EventBus>(Lifetime.Hierarchical);
            return configuration;
        }

        public static Configuration UseMockMessageQueueClient(this Configuration configuration)
        {
            IoCFactory.Instance.CurrentContainer.RegisterType<IMessageQueueClient, MockMessageQueueClient>(
                Lifetime.Singleton);
            return configuration;
        }

        public static Configuration UseMockCommandBus(this Configuration configuration)
        {
            IoCFactory.Instance.CurrentContainer.RegisterType<ICommandBus, MockCommandBus>(Lifetime.Singleton);
            return configuration;
        }

        public static Configuration UseMockMessagePublisher(this Configuration configuration)
        {
            IoCFactory.Instance.CurrentContainer.RegisterType<IMessagePublisher, MockMessagePublisher>(
                Lifetime.Singleton);
            return configuration;
        }

        public static Configuration UseMessagePublisher(this Configuration configuration, string defaultTopic)
        {
            var container = IoCFactory.Instance.CurrentContainer;
            var messageQueueClient = IoCFactory.Resolve<IMessageQueueClient>();
            configuration.SetDefaultTopic(defaultTopic);
            defaultTopic = configuration.FormatAppName(defaultTopic);
            //var messagePublisher = new
            return configuration;
        }

        public static Configuration SetAppNameFormat(this Configuration configuration, string format)
        {
            _appNameFormat = format;
            return configuration;
        }

        public static string FormatAppName(this Configuration configuration, string topic)
        {
            return string.IsNullOrEmpty(_appNameFormat) ? topic : string.Format(_appNameFormat, topic);
        }

        public static string FormatMessageQueueName(this Configuration configuration, string name)
        {
            return string.IsNullOrEmpty(_messageQueueNameFormat) ? name : string.Format(_messageQueueNameFormat);
        }
    }
}