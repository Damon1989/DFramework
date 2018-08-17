using DFramework.Message;

namespace DFramework.MessageQueue
{
    public interface ICommitOffsetable
    {
        string Id { get; }

        void CommitOffset(IMessageContext messageContext);

        void Start();

        void Stop();
    }
}