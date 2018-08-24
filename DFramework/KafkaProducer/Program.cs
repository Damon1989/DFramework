using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KafkaNet;
using KafkaNet.Common;
using KafkaNet.Model;
using KafkaNet.Protocol;

namespace KafkaProducer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            do
            {
                Produce(GetKafkaBroker(), GetTopicName());
                System.Threading.Thread.Sleep(3000);
            } while (true);
        }

        private static void Produce(string broker, string topic)
        {
            var options = new KafkaOptions(new Uri(broker));
            var router = new BrokerRouter(options);
            var client = new Producer(router);

            var currentDatetime = DateTime.Now;
            var key = currentDatetime.Second.ToString();
            var events = new[] { new Message($"Hello World" + currentDatetime, key) };
            client.SendMessageAsync(topic, events).Wait(1500);
            Console.WriteLine($"Produced:Key:{key}. Message:{events[0].Value.ToUtf8String()}");

            using (client)
            {
            }
        }

        private static string GetKafkaBroker()
        {
            string kafkaBroker = string.Empty;
            const string kafkaBrokerKeyName = "KafkaBroker";

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
            var topicName = string.Empty;
            const string topicNameKeyName = "Topic";
            if (!ConfigurationManager.AppSettings.AllKeys.Contains(topicNameKeyName))
            {
                throw new Exception("Key \"" + topicNameKeyName + "\" not found in Config file -> configuration/AppSettings");
            }
            else
            {
                topicName = ConfigurationManager.AppSettings[topicNameKeyName];
            }
            return topicName;
        }
    }
}