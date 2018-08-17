using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFramework.Config;
using DFramework.Infrastructure;

namespace DFramework.Message.Impl
{
    public static class MessageExtension
    {
        public static string GetTopic(this IMessage message)
        {
            string topic = null;
            var topicAttribute = message.GetCustomAttribute<TopicAttribute>();
            if (topicAttribute != null && !string.IsNullOrWhiteSpace(topicAttribute.Topic))
            {
                topic = topicAttribute.Topic;
            }

            if (string.IsNullOrEmpty(topic))
            {
                //topic=Configuration.Instance.getde
            }

            return topic;
        }

        public static string GetFormatTopic(this IMessage message)
        {
            var topic = message.GetTopic();
            if (!string.IsNullOrEmpty(topic))
            {
            }

            return topic;
        }
    }
}