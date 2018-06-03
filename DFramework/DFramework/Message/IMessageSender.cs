using System.Threading;
using System.Threading.Tasks;

namespace DFramework.Message
{
    public interface IMessageSender
    {
        void Start();

        void Stop();

        Task<MessageResponse[]> SendAsync(CancellationToken sendCancellationToken, params IMessage[] events);

        Task<MessageResponse[]> SendAsync(params IMessage[] events);

        Task<MessageResponse[]> SendAsync(params MessageState[] messageStates);
    }
}