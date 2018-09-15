using System;
using System.Linq;
using System.Threading.Tasks;
using KafkaNet;
using KafkaNet.Common;
using KafkaNet.Model;
using KafkaNet.Protocol;

namespace DFramework.KafkaNet
{
    internal class Program
    {
        //https://www.cnblogs.com/Wulex/p/5578339.html
        private static void Main(string[] args)
        {
            const string topicName = "test0905";//主题
            var options = new KafkaOptions(new Uri("http://localhost:9092"))
            {
                Log = new ConsoleLog()
            };

            Task.Run(() =>
            {
                //创建消费者
                var consumer =
                    new Consumer(new ConsumerOptions(topicName, new BrokerRouter(options)) { Log = new ConsoleLog() });
                foreach (var data in consumer.Consume())
                {
                    Console.WriteLine($"Response:PartitionId={data.Meta.PartitionId} Offset={data.Meta.Offset}: Value={data.Value.ToUtf8String()}");
                }
            });

            //创建一个生产者发消息
            var producer = new Producer(new BrokerRouter(options))
            {
                BatchSize = 100,
                BatchDelayTime = TimeSpan.FromMilliseconds(2000)
            };

            Console.WriteLine("打出一条消息按 enter...");

            while (true)
            {
                var message = Console.ReadLine();
                if (message == "quit") break;
                if (string.IsNullOrEmpty(message))
                {
                    SendRandomBatch(producer, topicName, 200);
                }
                else
                {
                    producer.SendMessageAsync(topicName, new[] { new Message(message) });
                }
            }

            using (producer)
            {
            }
        }

        private static async void SendRandomBatch(Producer producer, string topicName, int count)
        {
            //发送多个消息
            var sendTask = producer.SendMessageAsync(topicName,
                Enumerable.Range(0, count).Select(x => new Message(x.ToString())));
            Console.WriteLine($"传送了 ${count} messages. Buffered:{producer.BufferCount} AsyncCount:{producer.AsyncCount}");

            var response = await sendTask;

            Console.WriteLine($"已完成批量发送：{count}.Buffered:{producer.BufferCount} AsyncCount:{producer.AsyncCount}");

            foreach (var result in response.OrderBy(x => x.PartitionId))
            {
                Console.WriteLine($"主题：{result.Topic} PartitionId:{result.PartitionId} Offset:{result.Offset}");
            }
        }
    }
}