using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.Message
{
    public class MessageResponse
    {
        private readonly bool _needRetry;
        private readonly Task<object> _replyTask;
        public IMessageContext MessageContext { get; set; }

        public MessageResponse(IMessageContext messageContext)
            : this(messageContext, null, false)
        {
        }

        public MessageResponse(IMessageContext messageContext, Task<object> replyTask, bool needReply)
        {
            MessageContext = messageContext;
            _replyTask = replyTask;
            _needRetry = needReply;
        }

        public Task<object> Reply
        {
            get
            {
                if (!_needRetry)
                {
                    throw new Exception("Current response is set not to need message reply!");
                }
                return _replyTask;
            }
        }

        public async Task<TResult> ReadAsAsync<TResult>()
        {
            var result = await Reply.ConfigureAwait(false);
            return (TResult)result;
        }

        //public async Task<TResult> ReadAsAsync<TResult>(TimeSpan timeout)
        //{
        //}
    }
}