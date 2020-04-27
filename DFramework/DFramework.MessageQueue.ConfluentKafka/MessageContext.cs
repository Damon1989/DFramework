using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFramework.Infrastructure;
using DFramework.Message;
using DFramework.Message.Impl;
using DFramework.MessageQueue.ConfluentKafka.MessageFormat;
using Newtonsoft.Json.Linq;
using DCommon;

namespace DFramework.MessageQueue.ConfluentKafka
{
    public class MessageContext : IMessageContext
    {
        private object _message;

        private SagaInfo _sagaInfo;

        public MessageContext(KafkaMessage kafkaMessage, int partition, long offset)
        {
            KafkaMessage = kafkaMessage;
            Offset = offset;
            Partition = partition;
            ToBeSentMessageContexts = new List<IMessageContext>();
        }

        public MessageContext(object message, string id = null)
        {
            KafkaMessage = new KafkaMessage();
            SentTime = DateTime.Now;
            if (!string.IsNullOrEmpty(id))
            {
                MessageId = id;
            }
            else if (message is IMessage)
            {
                MessageId = ((IMessage)message).Id;
            }
            else
            {
                MessageId = ObjectId.GenerateNewId().ToString();
            }
            ToBeSentMessageContexts = new List<IMessageContext>();
            if (message != null && message is IMessage)
            {
                Topic = (message as IMessage).GetTopic();
            }
        }

        public MessageContext(IMessage message, string replyToEndPoint, string key)
        : this(message, key)
        {
            ReplyToEndPoint = replyToEndPoint;
        }

        public KafkaMessage KafkaMessage { get; set; }
        public int Partition { get; set; }
        public List<IMessageContext> ToBeSentMessageContexts { get; set; }
        public long Offset { get; set; }
        public IDictionary<string, object> Headers => KafkaMessage.Headers;

        public SagaInfo SagaInfo
        {
            get
            {
                if (_sagaInfo == null)
                {
                    var sagaInfoJson = Headers.TryGetValue("SagaInfo") as JObject;
                    if (sagaInfoJson != null)
                    {
                        try
                        {
                            _sagaInfo = ((JObject)Headers.TryGetValue("SagaInfo")).ToObject<SagaInfo>();
                        }
                        catch (Exception e)
                        {
                        }
                    }
                }

                return _sagaInfo;
            }
            set => Headers["SagaInfo"] = _sagaInfo = value;
        }

        public string Key
        {
            get => (string)Headers.TryGetValue("Key");
            set => Headers["Key"] = value;
        }

        public string CorrelationId
        {
            get => (string)Headers.TryGetValue("CorrelationId");
            set => Headers["CorrelationId"] = value;
        }

        public string MessageId
        {
            get => (string)Headers.TryGetValue("MessageId");
            set => Headers["MessageId"] = value;
        }

        public string ReplyToEndPoint
        {
            get => (string)Headers.TryGetValue("ReplyToEndPoint");
            set => Headers["ReplyToEndPoint"] = value;
        }

        public object Reply { get; set; }

        public object Message
        {
            get
            {
                if (_message != null)
                {
                    return _message;
                }

                object messageType = null;
                if (Headers.TryGetValue("MessageType", out messageType) && messageType != null)
                {
                    var jsonValue = KafkaMessage.Payload;
                    _message = jsonValue.ToJsonObject(Type.GetType(messageType.ToString()));
                }

                return _message;
            }

            protected set
            {
                _message = value;
                KafkaMessage.Payload = value.ToJson();
                if (value != null)
                {
                    Headers["MessageType"] = value.GetType().AssemblyQualifiedName;
                }
            }
        }

        public DateTime SentTime
        {
            get => (DateTime)Headers.TryGetValue("SentTime");
            set => Headers["SentTime"] = value;
        }

        public string Topic
        {
            get => (string)Headers.TryGetValue("Topic");
            set => Headers["Topic"] = value;
        }

        public string IP
        {
            get => (string)Headers.TryGetValue("IP");
            set => Headers["IP"] = value;
        }

        public string Producer
        {
            get => (string)Headers.TryGetValue("Producer");
            set => Headers["Producer"] = value;
        }
    }
}