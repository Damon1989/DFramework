using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DFramework.Message;
using DFramework.Message.Impl;

namespace DFramework.Command.Impl
{
    public class MockCommandBus : ICommandBus
    {
        public void Start()
        {
        }

        public void Stop()
        {
        }

        public Task<MessageResponse> StartSaga(ICommand command, string sagaId = null)
        {
            throw new NotImplementedException();
        }

        public Task<MessageResponse> StartSaga(ICommand command, TimeSpan timeout, string sagaId = null)
        {
            throw new NotImplementedException();
        }

        public Task<MessageResponse> StartSaga(ICommand command, CancellationToken sendCancellationToken, TimeSpan sendTimeout,
            CancellationToken replyCancellationToken, string sagaId = null)
        {
            throw new NotImplementedException();
        }

        public Task<MessageResponse> SendAsync(ICommand command, bool needReply = false)
        {
            throw new NotImplementedException();
        }

        public Task<MessageResponse> SendAsync(ICommand command, TimeSpan timeout, bool needReply = false)
        {
            throw new NotImplementedException();
        }

        public Task<MessageResponse> SendAsync(ICommand command, CancellationToken sendCancellationToken, TimeSpan sendTimeout,
            CancellationToken replyCancellationToken, bool needReply = false)
        {
            throw new NotImplementedException();
        }

        public void SendMessageStates(IEnumerable<MessageState> messageStates)
        {
        }

        public IMessageContext WrapCommand(ICommand command, bool needReply = false, SagaInfo sagaInfo = null, string producer = null)
        {
            throw new NotImplementedException();
        }
    }
}