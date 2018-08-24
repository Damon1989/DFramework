using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KafkaNet;
using KafkaNet.Common;
using KafkaNet.Model;

namespace KafkaConsumer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Consume(GetKafkaBroker(), GetTopicName());
        }

        private static void Consume(string broker, string topic)
        {
            var options = new KafkaOptions(new Uri(broker));
            var router = new BrokerRouter(options);

            var consumer = new Consumer(new ConsumerOptions(topic, router));

            foreach (var message in consumer.Consume())
            {
                Console.WriteLine($"Response: Partition {message.Meta.PartitionId}, Offset {message.Meta.Offset}:{message.Value.ToUtf8String()}");
            }
        }

        private static string GetKafkaBroker()
        {
            var kafkaBroker = string.Empty;
            var kafkaBrokerKeyName = "KafkaBroker";
            if (!ConfigurationManager.AppSettings.AllKeys.Contains(kafkaBrokerKeyName))
            {
                kafkaBroker = "http://localhost:9092";
            }
            else
            {
                kafkaBroker = ConfigurationManager.AppSettings[kafkaBrokerKeyName];
            }
            return kafkaBroker;
        }

        private static string GetTopicName()
        {
            string topicName = string.Empty;
            var topicNameKeyName = "Topic";

            if (!ConfigurationManager.AppSettings.AllKeys.Contains(topicNameKeyName))
            {
                throw new Exception($"Key{ topicNameKeyName} not found in Config file -> configuration/AppSettings");
            }
            else
            {
                topicName = ConfigurationManager.AppSettings[topicNameKeyName];
            }
            return topicName;
        }
    }
}