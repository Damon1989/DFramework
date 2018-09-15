using System;
using System.Configuration;
using System.Linq;
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

            //Consume returns a blocking IEnumerable(ie: never ending stream)
            foreach (var message in consumer.Consume())
            {
                Console.WriteLine($"Response: Partition {message.Meta.PartitionId}, Offset {message.Meta.Offset}:{message.Value.ToUtf8String()}");
            }
        }

        private static string GetKafkaBroker()
        {
            var kafkaBrokerKeyName = "KafkaBroker";
            var kafkaBroker = ConfigurationManager.AppSettings.AllKeys.Contains(kafkaBrokerKeyName)
                ? ConfigurationManager.AppSettings[kafkaBrokerKeyName]
                : "http://localhost:9092";
            return kafkaBroker;
        }

        private static string GetTopicName()
        {
            var topicNameKeyName = "Topic";

            if (!ConfigurationManager.AppSettings.AllKeys.Contains(topicNameKeyName))
            {
                throw new Exception($"Key{ topicNameKeyName} not found in Config file -> configuration/AppSettings");
            }

            var topicName = ConfigurationManager.AppSettings[topicNameKeyName];
            return topicName;
        }
    }
}