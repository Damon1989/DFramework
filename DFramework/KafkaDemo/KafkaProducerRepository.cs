using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return context.KafkaProducerMessage.ToList();
        }

        public List<string> GetDistinctTopics()
        {
            return context.KafkaProducerMessage.Select(a => a.Topic)
                .Distinct()
                .ToList();
        }

        public KafkaProducerMessage GetKafkaProducerMessageByMessageID(int MessageID)
        {
            return context.KafkaProducerMessage.Find(MessageID);
        }

        public List<KafkaProducerMessage> GetKafkaProducerMessageByTopic(string TopicName)
        {
            return context.KafkaProducerMessage
                     .Where(a => a.Topic == TopicName)
                     .OrderBy(a => a.KafkaProducerMessageId)
                     .ToList<KafkaProducerMessage>();
        }

        public void InsertKafkaProducerMessage(KafkaProducerMessage Message)
        {
            context.KafkaProducerMessage.Add(Message);
            context.SaveChanges();
        }

        public void ArchiveKafkaProducerMessage(int MessageID)
        {
            KafkaProducerMessage m = context.KafkaProducerMessage.Find(MessageID);
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

        public void ArchiveKafkaProducerMessageList(List<KafkaProducerMessage> Messages)
        {
            foreach (KafkaProducerMessage Message in Messages)
            {
                ArchiveKafkaProducerMessage(Message.KafkaProducerMessageId);
            }
        }

        public static KafkaProducerMessageArchive KafkaProducerMessageToKafkaProducerMessageArchive(KafkaProducerMessage Message)
        {
            return new KafkaProducerMessageArchive
            {
                KafkaProducerMessageId = Message.KafkaProducerMessageId,
                Content = Message.Content,
                Topic = Message.Topic,
                CreatedTime = Message.CreatedTime,
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