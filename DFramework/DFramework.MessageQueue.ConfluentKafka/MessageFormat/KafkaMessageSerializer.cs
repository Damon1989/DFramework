using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka.Serialization;
using DFramework.Infrastructure;

namespace DFramework.MessageQueue.ConfluentKafka.MessageFormat
{
    public class KafkaMessageSerializer : ISerializer<KafkaMessage>
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public byte[] Serialize(string topic, KafkaMessage data)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, object>> Configure(IEnumerable<KeyValuePair<string, object>> config, bool isKey)
        {
            throw new NotImplementedException();
        }

        public byte[] Serialize(KafkaMessage data)
        {
            var jsonValue = data.ToJson();
            return Encoding.UTF8.GetBytes(jsonValue);
        }
    }
}