using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace KafkaDemo
{
    public class KafkaProducerRepository
    {
        private KafkaModel context;

        public KafkaProducerRepository()
        {
            this.context = new KafkaModel();
        }

        public List<KafkaProducerMessage> GetKafkaProducerMessages()
        {
            return context.KafkaProducerMessage.AsNoTracking().ToList();
        }

        public List<string> GetDistinctTopics()
        {
            return context.KafkaProducerMessage.Select(a => a.Topic)
                .Distinct()
                .ToList();
        }

        public KafkaProducerMessage GetKafkaProducerMessageByMessageId(int MessageId)
        {
            return context.KafkaProducerMessage.Find(MessageId);
        }

        public List<KafkaProducerMessage> GetKafkaProducerMessageByTopic(string topicName)
        {
            return context.KafkaProducerMessage
                     .Where(a => a.Topic == topicName)
                     .OrderBy(a => a.KafkaProducerMessageId)
                     .ToList<KafkaProducerMessage>();
        }

        public void InsertKafkaProducerMessage(KafkaProducerMessage message)
        {
            context.KafkaProducerMessage.Add(message);
            context.SaveChanges();
        }

        public void ArchiveKafkaProducerMessage(int messageId)
        {
            KafkaProducerMessage m = context.KafkaProducerMessage.Find(messageId);
            KafkaProducerMessageArchive archivedMessage = KafkaProducerMessageToKafkaProducerMessageArchive(m);

            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.KafkaProducerMessageArchive.Add(archivedMessage);
                    context.KafkaProducerMessage.Remove(m);
                    dbContextTransaction.Commit();
                    context.SaveChanges();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    context.SaveChanges();
                }
            }
        }

        public void ArchiveKafkaProducerMessageList(List<KafkaProducerMessage> messages)
        {
            foreach (KafkaProducerMessage message in messages)
            {
                ArchiveKafkaProducerMessage(message.KafkaProducerMessageId);
            }
        }

        public static KafkaProducerMessageArchive KafkaProducerMessageToKafkaProducerMessageArchive(KafkaProducerMessage message)
        {
            return new KafkaProducerMessageArchive
            {
                KafkaProducerMessageId = message.KafkaProducerMessageId,
                Content = message.Content,
                Topic = message.Topic,
                CreatedTime = message.CreatedTime,
                ArchivedTime = DateTime.UtcNow
            };
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