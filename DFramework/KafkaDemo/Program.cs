using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KafkaNet;
using KafkaNet.Model;
using KafkaNet.Protocol;

namespace KafkaDemo
{
    internal class Program
    {
        private static string _topic;

        private static void Main(string[] args)
        {
            var header = "kafka测试";
            Console.Title = header;
            Console.WriteLine(header);
            ConsoleColor color = Console.ForegroundColor;

            var pub = new KafkaHelper("Test", true);

            var sub = new KafkaHelper("Test", false);

            Task.Run(() =>
            {
                while (true)
                {
                    string msg = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}这是一条测试消息";
                    pub.Pub(new List<string>() { msg });

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"发送消息{msg}");

                    Thread.Sleep(2000);
                }
            });

            Task.Run(() =>
            {
                sub.Sub(msg =>
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"收到消息{msg}");
                });
            });

            Console.ReadLine();

            //string invalidArgErrorMessage = "有效的args是：produce或consume";

            //if (args.Length < 1)
            //{
            //    throw (new Exception(invalidArgErrorMessage));
            //}

            //string intent = args[1];

            //_topic = ConfigurationManager.AppSettings["Topic"];
            //if (String.Equals(intent, "consume", StringComparison.OrdinalIgnoreCase))
            //{
            //    Console.WriteLine("开始消费者服务");
            //    Consume();
            //}
            //else if (String.Equals(intent, "produce", StringComparison.OrdinalIgnoreCase))
            //{
            //    Console.WriteLine("开始生产者服务");
            //    Produce();
            //}
            //else
            //{
            //    throw (new Exception(invalidArgErrorMessage));
            //}
        }

        private static BrokerRouter InitDefaultConfig()
        {
            var options = new KafkaOptions(ConfigurationManager.AppSettings["BrokerList"].Split(',').Select(item => new Uri(item)).ToArray());
            var router = new BrokerRouter(options);
            return router;
        }

        private static void Consume()
        {
            var kafkaRepo = new KafkaConsumerRepository();
            bool fromBeginning = Boolean.Parse(ConfigurationManager.AppSettings["FromBeginning"]);

            var router = InitDefaultConfig();
            var consumer = new Consumer(new ConsumerOptions(_topic, router));

            //如果我们不想从头开始，使用最新偏移量
            if (!fromBeginning)
            {
                var maxOffsetByPartition = kafkaRepo.GetOffsetPositionByTopic(_topic);
                //如果我们得到一个结果使用它，否则默认
                if (maxOffsetByPartition.Any())
                {
                    var offsets = new List<OffsetPosition>();
                    foreach (var m in maxOffsetByPartition)
                    {
                        var o = new OffsetPosition(m.Partition, (long)m.MaxOffset + 1);
                        offsets.Add(o);
                    }
                    consumer.SetOffsetPosition(offsets.ToArray());
                }
                else
                {
                    consumer.SetOffsetPosition(new OffsetPosition());
                }
            }

            //消耗返回一个阻塞IEnumerable(ie:从没有结束流)
            foreach (var message in consumer.Consume())
            {
                var messageContent = Encoding.UTF8.GetString(message.Value);

                Console.WriteLine($"处理带有内容的消息:{messageContent}");

                kafkaRepo = new KafkaConsumerRepository();

                var consumerMessage = new KafkaConsumerMessage()
                {
                    Topic = _topic,
                    Offset = (int)message.Meta.Offset,
                    Partition = message.Meta.PartitionId,
                    Content = messageContent,
                    CreatedTime = DateTime.UtcNow
                };

                kafkaRepo.InsertKafkaConsumerMessage(consumerMessage);
                kafkaRepo.Dispose();
            }
        }

        private static void Produce()
        {
            var kafkaRepo = new KafkaProducerRepository();
            var router = InitDefaultConfig();
            var client = new Producer(router);

            var messages = new List<Message>();

            foreach (var message in kafkaRepo.GetKafkaProducerMessageByTopic(_topic))
            {
                messages.Add(new Message(message.Content));
                kafkaRepo.ArchiveKafkaProducerMessage(message.KafkaProducerMessageId);
            }

            client.SendMessageAsync(_topic, messages).Wait();

            kafkaRepo.Dispose();
        }

        public static void KafkaTest()
        {
            var header = "kafka测试";

            Console.Title = header;
            Console.WriteLine(header);
            ConsoleColor color = Console.ForegroundColor;

            var pub = new KafkaHelper("Test", true);

            var sub = new KafkaHelper("Test", false);

            Task.Run(() =>
            {
                while (true)
                {
                    var msg = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}这是一条测试消息";

                    pub.Pub(new List<string> { msg });
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"发消息：{msg}");

                    Thread.Sleep(2000);
                }
            });

            Task.Run(() =>
            {
                sub.Sub(msg =>
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"收到消息:{msg}");
                });
            });

            Console.ReadLine();
        }
    }
}