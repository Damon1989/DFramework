using System.Collections.Generic;

namespace DFramework.MessageQueue.ConfluentKafka.MessageFormat
{
    public class KafkaMessage
    {
        public KafkaMessage(string payload = null)
        {
            Headers = new Dictionary<string, object>();
            Payload = payload;
        }

        public IDictionary<string, object> Headers { get; set; }
        public string Payload { get; set; }
    }
}