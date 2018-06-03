using System.Threading.Tasks;

namespace DFramework.Message
{
    public class MessageState
    {
        public MessageState(IMessageContext messageContext,
            bool needRetry = false)
            : this(messageContext, null, needRetry)
        {
        }

        public MessageState(IMessageContext messageContext,
            TaskCompletionSource<MessageResponse> sendTaskCompletionSource,
            bool needRetry)
            : this(messageContext, sendTaskCompletionSource, null, needRetry)
        {
        }

        public MessageState(IMessageContext messageContext,
            TaskCompletionSource<MessageResponse> sendTaskCompletionSource,
            TaskCompletionSource<object> replyTaskCompletionSource,
            bool needReply)
        {
            MessageContext = messageContext;
            MessageID = messageContext.MessageId;
            NeedRetry = needReply;
            SendTaskCompletionSource = sendTaskCompletionSource;
            ReplyTaskCompletionSource = replyTaskCompletionSource;
        }

        public string MessageID { get; set; }
        public bool NeedRetry { get; set; }
        public IMessageContext MessageContext { get; set; }

        public TaskCompletionSource<object> ReplyTaskCompletionSource { get; set; }
        public TaskCompletionSource<MessageResponse> SendTaskCompletionSource { get; set; }
    }
}