using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DFramework.Config;
using DFramework.Infrastructure.Logging;
using DFramework.IoC;
using DFramework.Message;
using DFramework.Message.Impl;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace DFramework.MessageQueue.ServiceBus
{
    public class ServiceBusClient : IMessageQueueClient
    {
        protected ILogger _logger;
        protected MessagingFactory _messagingFactory;
        protected NamespaceManager _namespaceManager;
        protected ConcurrentDictionary<string, QueueClient> _queueClients;
        protected string _serviceBusConnectionString;
        protected ConcurrentDictionary<string, TopicClient> _topicClients;

        public ServiceBusClient(string serviceBusConnectionString)
        {
            _serviceBusConnectionString = serviceBusConnectionString;
            _namespaceManager = NamespaceManager.CreateFromConnectionString(_serviceBusConnectionString);
            _messagingFactory = MessagingFactory.CreateFromConnectionString(_serviceBusConnectionString);
            _topicClients = new ConcurrentDictionary<string, TopicClient>();
            _queueClients = new ConcurrentDictionary<string, QueueClient>();
            _logger = IoCFactory.Resolve<ILoggerFactory>().Create(GetType());
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(IMessageContext messageContext, string queue, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task PublishAsync(IMessageContext messageContext, string topic, CancellationToken cancellationToken)
        {
            //topic = Configuration.Instance.FormatMessageQueueName(topic);
            //var topicClient = GetTopicClient(topic);

            throw new NotImplementedException();
        }

        private TopicClient GetTopicClient(string topic)
        {
            TopicClient topicClient = null;
            _topicClients.TryGetValue(topic, out topicClient);
            if (topicClient == null)
            {
                topicClient = CreateTopicClient(topic);
                _topicClients.GetOrAdd(topic, topicClient);
            }

            return topicClient;
        }

        private TopicClient CreateTopicClient(string topicName)
        {
            var td = new TopicDescription(topicName);
            if (!_namespaceManager.TopicExists(topicName))
            {
                _namespaceManager.CreateTopic(td);
            }

            return _messagingFactory.CreateTopicClient(topicName);
        }

        public IMessageContext WarpMessage(object message, string correlationnId = null, string topic = null, string key = null,
            string replyEndPoint = null, string messageId = null, SagaInfo sagaInfo = null, string producer = null)
        {
            throw new NotImplementedException();
        }
    }
}