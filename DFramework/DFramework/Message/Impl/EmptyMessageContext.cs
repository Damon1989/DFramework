using System;
using System.Collections.Generic;

namespace DFramework.Message.Impl
{
    public class EmptyMessageContext : IMessageContext
    {
        public EmptyMessageContext()
        {
        }

        public EmptyMessageContext(IMessage message)
        {
            SentTime = DateTime.Now;
            Message = message;
            MessageId = message.Id;
        }

        public IDictionary<string, object> Headers => null;
        public string Key => null;
        public string MessageId { get; }
        public string CorrelationId { get; set; }
        public string ReplyToEndPoint => null;
        public object Reply { get; set; }
        public object Message { get; set; }
        public DateTime SentTime { get; set; }
        public string Topic { get; set; }
        public long Offset { get; set; }
        public SagaInfo SagaInfo => null;
        public string IP { get; set; }
        public string Producer { get; set; }
    }
}