using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DFramework.IoC;
using DFramework.MessageQueue;

namespace DFramework.Message.Impl
{
    public class MessagePublisher : MessageSender, IMessagePublisher
    {
        public MessagePublisher(IMessageQueueClient messageQueueClient, string defaultTopic)
            : base(messageQueueClient, defaultTopic)
        {
            if (string.IsNullOrEmpty(defaultTopic))
            {
                throw new Exception("message sender must have a default topic.");
            }
        }

        protected override IEnumerable<IMessageContext> GetAllUnSentMessages()
        {
            using (var scope = IoCFactory.Instance.CurrentContainer.CreateChildContainer())
            {
                using (var messageStore = scope.Resolve<IMessageStore>())
                {
                    return messageStore.GetAllUnPublishedEvents(
                        (messageId, message, topic, correlationId, replyEndPoint, sagaInfo, producer) => _messageQueueClient.WarpMessage(message, key: message.Key,
                            topic: topic, messageId: messageId, correlationnId: correlationId,
                            replyEndPoint: replyEndPoint, sagaInfo: sagaInfo, producer: producer));
                }
            }
        }

        protected override async Task SendMessageStateAsync(MessageState messageState, CancellationToken cancellationToken)
        {
            var messageContext = messageState.MessageContext;
            await _messageQueueClient.PublishAsync(messageContext, messageContext.Topic ?? _defaultTopic,
                cancellationToken);
            CompleteSendingMessage(messageState);
        }

        protected override void CompleteSendingMessage(MessageState messageState)
        {
            messageState.SendTaskCompletionSource?.TrySetResult(new MessageResponse(messageState.MessageContext, null,
                false));

            if (_needMessageStore)
            {
                Task.Run(() =>
                {
                    using (var scope = IoCFactory.Instance.CurrentContainer.CreateChildContainer())
                    {
                        using (var messageStore = scope.Resolve<IMessageStore>())
                        {
                            messageStore.RemovePublishedEvent(messageState.MessageId);
                        }
                    }
                });
            }
        }
    }
}