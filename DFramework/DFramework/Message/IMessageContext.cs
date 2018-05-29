using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFramework.Message.Impl;

namespace DFramework.Message
{
    public interface IMessageContext
    {
        IDictionary<string, object> Headers { get; }
        string Key { get; }
        string MessageId { get; }
        string CorrelationId { get; set; }
        string ReplyToEndPoint { get; }
        object Reply { get; }
        object Message { get; }
        DateTime SentTime { get; }
        string Topic { get; }
        long Offset { get; }
        SagaInfo SagaInfo { get; }
        string IP { get; set; }
        string Producer { get; set; }
    }
}