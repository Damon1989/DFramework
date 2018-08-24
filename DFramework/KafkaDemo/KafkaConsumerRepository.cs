using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaDemo
{
    public class KafkaConsumerRepository
    {
        private KafkaModel context;

        public KafkaConsumerRepository()
        {
            this.context = new KafkaModel();
        }

        public List<KafkaConsumerMessage> GetKafkaConsumerMessages()
        {
            return context.KafkaConsumerMessage.ToList();
        }

        public KafkaConsumerMessage GetKafkaConsumerMessageByID(int MessageID)
        {
            return context.KafkaConsumerMessage.Find(MessageID);
        }

        public List<KafkaConsumerMessage> GetKafkaConsumerMessageByTopic(string TopicName)
        {
            return context.KafkaConsumerMessage
                .Where(a => a.Topic == TopicName)
                .ToList();
        }

        public List<VwMaxOffsetByPartitionAndTopic> GetOffsetPositionByTopic(string TopicName)
        {
            return context.VwMaxOffsetByPartitionAndTopic
                .Where(a => a.Topic == TopicName)
                .ToList();
        }

        public void InsertKafkaConsumerMessage(KafkaConsumerMessage Message)
        {
            Console.WriteLine($"Insert {Message.KafkaConsumerMessageId.ToString()}: {Message.Content}");
            context.KafkaConsumerMessage.Add(Message);
            context.SaveChanges();

            Console.WriteLine($"Saved {Message.KafkaConsumerMessageId.ToString()}: {Message.Content}");
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}