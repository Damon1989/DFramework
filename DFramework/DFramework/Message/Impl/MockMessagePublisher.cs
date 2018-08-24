using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DFramework.Message.Impl
{
    public class MockMessagePublisher : IMessagePublisher
    {
        public void Start()
        {
        }

        public void Stop()
        {
        }

        public Task<MessageResponse[]> SendAsync(CancellationToken sendCancellationToken, params IMessage[] events)
        {
            return null;
        }

        public Task<MessageResponse[]> SendAsync(params IMessage[] events)
        {
            return null;
        }

        public Task<MessageResponse[]> SendAsync(params MessageState[] messageStates)
        {
            return null;
        }
    }
}