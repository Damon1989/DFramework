using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DFramework.Config;
using DFramework.Infrastructure;
using DFramework.Infrastructure.Logging;
using DFramework.IoC;
using DFramework.MessageQueue;
using DCommon;

namespace DFramework.Message.Impl
{
    public abstract class MessageSender : IMessageSender
    {
        protected string _defaultTopic;
        protected ILogger _logger;
        protected IMessageQueueClient _messageQueueClient;
        protected bool _needMessageStore;
        protected Task _sendMessageTask;

        public MessageSender(IMessageQueueClient messageQueueClient, string defaultTopic = null)
        {
            _messageQueueClient = messageQueueClient;
            _defaultTopic = defaultTopic;
            _needMessageStore = Configuration.Instance.NeedMessageStore;
            _messageStateQueue = new BlockingCollection<MessageState>();
            _logger = IoCFactory.IsInit() ? IoCFactory.Resolve<ILoggerFactory>().Create(GetType().Name) : null;
        }

        protected BlockingCollection<MessageState> _messageStateQueue { get; set; }

        public void Start()
        {
            if (_needMessageStore)
            {
                GetAllUnSentMessages().ForEach(eventContext => _messageStateQueue.Add(new MessageState(eventContext)));

                var cancellationTokenSource = new CancellationTokenSource();
                _sendMessageTask = Task.Factory.StartNew(cs => SendMessage(cs as CancellationTokenSource),
                    cancellationTokenSource,
                    cancellationTokenSource.Token,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default);
            }
        }

        protected abstract IEnumerable<IMessageContext> GetAllUnSentMessages();

        protected abstract Task SendMessageStateAsync(MessageState messageState, CancellationToken cancellationToken);

        protected abstract void CompleteSendingMessage(MessageState messageState);

        public virtual void Stop()
        {
            if (_sendMessageTask != null)
            {
                var cancellationSource = _sendMessageTask.AsyncState as CancellationTokenSource;
                cancellationSource.Cancel(true);
                Task.WaitAll(_sendMessageTask);
            }
        }

        public Task<MessageResponse[]> SendAsync(CancellationToken sendCancellationToken, params IMessage[] events)
        {
            var sendTaskCompletionSource = new TaskCompletionSource<MessageResponse>();
            if (sendCancellationToken != CancellationToken.None)
            {
                sendCancellationToken.Register(OnSendCancel, sendTaskCompletionSource);
            }

            var messageStates = events.Select(message =>
            {
                var topic = message.GetFormatTopic();
                return new MessageState(_messageQueueClient.WarpMessage(message, topic: topic, key: message.Key),
                    sendTaskCompletionSource, false);
            }).ToArray();
            return SendAsync(messageStates);
        }

        public Task<MessageResponse[]> SendAsync(params IMessage[] events)
        {
            return SendAsync(CancellationToken.None, events);
        }

        public Task<MessageResponse[]> SendAsync(params MessageState[] messageStates)
        {
            messageStates.ForEach(messageState => { _messageStateQueue.Add(messageState); });
            messageStates.ForEach(ms =>
            {
                _logger.Debug($"send message enqueue msgIdL{ms.MessageId} topic:{ms.MessageContext.Topic}");
            });
            return Task.WhenAll(messageStates.Where(s => s.SendTaskCompletionSource != null)
                .Select(s => s.SendTaskCompletionSource.Task)
                .ToArray());
        }

        protected virtual void OnSendCancel(object state)
        {
            var sendTaskCompletionSource = state as TaskCompletionSource<MessageResponse>;
            sendTaskCompletionSource?.TrySetCanceled();
        }

        private void SendMessage(CancellationTokenSource cancellationTokenSource)
        {
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    var messageState = _messageStateQueue.Take(cancellationTokenSource.Token);
                    SendMessageStateAsync(messageState, cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch (ThreadAbortException)
                {
                    return;
                }
                catch (Exception e)
                {
                    _logger?.Error(e);
                }
            }
        }
    }
}