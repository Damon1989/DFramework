using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFramework.Infrastructure;
using DFramework.Message;
using DFramework.Message.Impl;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json.Linq;
using DCommon;

namespace DFramework.MessageQueue.ServiceBus.MessageFormat
{
    public class MessageContext : IMessageContext
    {
        public object Message { get; }
        public DateTime SentTime { get; }
        public string Topic { get; }
        public long Offset { get; }
        public string IP { get; set; }
        public string Producer { get; set; }

        private object _message;

        private SagaInfo _sagaInfo;

        public BrokeredMessage BrokeredMessage { get; set; }
        public List<IMessageContext> ToBeSentMessageContexts { get; protected set; }
        public Action Complete { get; protected set; }

        public MessageContext(BrokeredMessage brokeredMessage, Action complete = null)
        {
            BrokeredMessage = brokeredMessage;
            SentTime = DateTime.Now;
            Complete = complete;
            Offset = brokeredMessage.SequenceNumber;
            ToBeSentMessageContexts = new List<IMessageContext>();
        }

        public MessageContext(object message, string id = null)
        {
            BrokeredMessage = new BrokeredMessage(message.ToJson());
            SentTime = DateTime.Now;
            Message = message;
            if (!string.IsNullOrEmpty(id))
            {
                MessageId = id;
            }
            else if (message is IMessage)
            {
                MessageId = (message as IMessage).Id;
            }
            else
            {
                MessageId = ObjectId.GenerateNewId().ToString();
            }

            ToBeSentMessageContexts = new List<IMessageContext>();
            if (message != null)
            {
                var topicAttribute = message.GetCustomAttribute<TopicAttribute>();
                if (topicAttribute != null && !string.IsNullOrWhiteSpace(topicAttribute.Topic))
                {
                    Topic = topicAttribute.Topic;
                }
            }
        }

        public string ReplyToEndPoint
        {
            get => BrokeredMessage.ReplyTo;
            set => BrokeredMessage.ReplyTo = value;
        }

        public MessageContext(IMessage message, string key)
        : this(message)
        {
            Key = key;
        }

        public MessageContext(IMessage message, string replyToEndPoint, string key)
            : this(message, key)
        {
            ReplyToEndPoint = replyToEndPoint;
        }

        public IDictionary<string, object> Headers => BrokeredMessage.Properties;

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
            get => BrokeredMessage.CorrelationId;
            set => BrokeredMessage.CorrelationId = value;
        }

        public string MessageId
        {
            get => BrokeredMessage.MessageId;
            set => BrokeredMessage.MessageId = value;
        }

        public object Reply { get; set; }
    }
}