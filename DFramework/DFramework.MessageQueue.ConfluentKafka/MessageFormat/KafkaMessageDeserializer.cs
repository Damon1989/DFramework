using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka.Serialization;
using DFramework.Infrastructure;

namespace DFramework.MessageQueue.ConfluentKafka.MessageFormat
{
    public class KafkaMessageDeserializer : IDeserializer<KafkaMessage>
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public KafkaMessage Deserialize(byte[] data)
        {
            return Encoding.UTF8.GetString(data).ToJsonObject<KafkaMessage>();
        }

        public KafkaMessage Deserialize(string topic, byte[] data)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, object>> Configure(IEnumerable<KeyValuePair<string, object>> config, bool isKey)
        {
            throw new NotImplementedException();
        }
    }
}