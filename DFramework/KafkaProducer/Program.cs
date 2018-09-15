using System;
using System.Configuration;
using System.Linq;
using KafkaNet;
using KafkaNet.Common;
using KafkaNet.Model;
using KafkaNet.Protocol;

namespace KafkaProducer
{
    internal class Program
    {
        //https://www.cnblogs.com/Wulex/p/5619425.html#3936856
        private static void Main(string[] args)
        {
            var str = "1";
            foreach (var s in str.Split(','))
            {
                Console.WriteLine(s);
            }

            Console.ReadLine();
            //do
            //{
            //    Produce(GetKafkaBroker(), GetTopicName());
            //    System.Threading.Thread.Sleep(3000);
            //} while (true);
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
            const string kafkaBrokerKeyName = "KafkaBroker";

            var kafkaBroker = ConfigurationManager.AppSettings.AllKeys.Contains(kafkaBrokerKeyName)
                ? ConfigurationManager.AppSettings[kafkaBrokerKeyName]
                : "http://localhost:9092";

            return kafkaBroker;
        }

        private static string GetTopicName()
        {
            const string topicNameKeyName = "Topic";
            if (!ConfigurationManager.AppSettings.AllKeys.Contains(topicNameKeyName))
            {
                throw new Exception("Key \"" + topicNameKeyName + "\" not found in Config file -> configuration/AppSettings");
            }

            var topicName = ConfigurationManager.AppSettings[topicNameKeyName];
            return topicName;
        }
    }
}