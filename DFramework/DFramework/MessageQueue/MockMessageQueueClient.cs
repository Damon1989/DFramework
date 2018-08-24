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
    public class MockMessageQueueClient : IMessageQueueClient
    {
        public void Dispose()
        {
        }

        public Task SendAsync(IMessageContext messageContext, string queue, CancellationToken cancellationToken)
        {
            return Task.FromResult<object>(null);
        }

        public Task PublishAsync(IMessageContext messageContext, string topic, CancellationToken cancellationToken)
        {
            return Task.FromResult<object>(null);
        }

        public IMessageContext WarpMessage(object message, string correlationnId = null, string topic = null, string key = null,
            string replyEndPoint = null, string messageId = null, SagaInfo sagaInfo = null, string producer = null)
        {
            throw new NotImplementedException();
        }
    }
}