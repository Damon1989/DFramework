using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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
            return context.KafkaConsumerMessage.AsNoTracking().ToList();
        }

        public KafkaConsumerMessage GetKafkaConsumerMessageById(int MessageId)
        {
            return context.KafkaConsumerMessage.Find(MessageId);
        }

        public List<KafkaConsumerMessage> GetKafkaConsumerMessageByTopic(string topicName)
        {
            return context.KafkaConsumerMessage
                .Where(a => a.Topic == topicName)
                .AsNoTracking()
                .ToList();
        }

        public List<VwMaxOffsetByPartitionAndTopic> GetOffsetPositionByTopic(string topicName)
        {
            return context.VwMaxOffsetByPartitionAndTopic
                .Where(a => a.Topic == topicName)
                .AsNoTracking()
                .ToList();
        }

        public void InsertKafkaConsumerMessage(KafkaConsumerMessage message)
        {
            Console.WriteLine($"Insert {message.KafkaConsumerMessageId.ToString()}: {message.Content}");
            context.KafkaConsumerMessage.Add(message);
            context.SaveChanges();

            Console.WriteLine($"Saved {message.KafkaConsumerMessageId.ToString()}: {message.Content}");
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