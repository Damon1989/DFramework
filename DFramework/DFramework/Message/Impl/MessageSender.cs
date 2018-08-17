using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DFramework.Infrastructure.Logging;

namespace DFramework.Message.Impl
{
    public abstract class MessageSender : IMessageSender
    {
        protected string _defaultTopic;
        protected ILogger _logger;

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public Task<MessageResponse[]> SendAsync(CancellationToken sendCancellationToken, params IMessage[] events)
        {
            throw new NotImplementedException();
        }

        public Task<MessageResponse[]> SendAsync(params IMessage[] events)
        {
            throw new NotImplementedException();
        }

        public Task<MessageResponse[]> SendAsync(params MessageState[] messageStates)
        {
            throw new NotImplementedException();
        }
    }
}