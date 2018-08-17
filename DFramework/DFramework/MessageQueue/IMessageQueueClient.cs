using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DFramework.Message;
using DFramework.Message.Impl;

namespace DFramework.MessageQueue
{
    public delegate void OnMessagesReceived(params IMessageContext[] messageContext);

    public interface IMessageQueueClient : IDisposable
    {
        Task SendAsync(IMessageContext messageContext, string queue, CancellationToken cancellationToken);

        Task PublishAsync(IMessageContext messageContext, string topic, CancellationToken cancellationToken);

        IMessageContext WarpMessage(object message,
            string correlationnId = null,
            string topic = null,
            string key = null,
            string replyEndPoint = null,
            string messageId = null,
            SagaInfo sagaInfo = null,
            string producer = null);
    }
}