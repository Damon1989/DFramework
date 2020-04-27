using System;
using System.CodeDom;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using DFramework.Config;
using DFramework.Infrastructure;
using DFramework.Infrastructure.Logging;
using DFramework.IoC;
using DFramework.Message;
using DCommon;

namespace DFramework.MessageQueue.ConfluentKafka
{
    public delegate void OnKafkaMessageReceived<TKey, TValue>(KafkaConsumer<TKey, TValue> consumer,
        Message<TKey, TValue> message);

    public class KafkaConsumer<TKey, TValue> : ICommitOffsetable
    {
        protected CancellationTokenSource _cancellationTokenSource;
        private Consumer<TKey, TValue> _consumer;
        protected Task _consumerTask;

        protected ILogger _logger =
            IoCFactory.Resolve<ILoggerFactory>().Create(typeof(KafkaConsumer<TKey, TValue>).Name);

        protected OnKafkaMessageReceived<TKey, TValue> _onMessageReceived;

        protected ConsumerConfig _consumerConfig;

        private readonly IDeserializer<TKey> _keyDeserializer;
        private readonly IDeserializer<TValue> _valueDeserializer;

        public KafkaConsumer(string brokerList,
            string topic,
            string groupId,
            string consumerId,
            OnKafkaMessageReceived<TKey, TValue> onMessageReceived,
            IDeserializer<TKey> keyDeserializer,
            IDeserializer<TValue> valueDeserializer,
            ConsumerConfig consumerConfig = null,
            bool start = true)
        {
            _keyDeserializer = keyDeserializer ?? throw new ArgumentNullException(nameof(keyDeserializer));
            _valueDeserializer = valueDeserializer ?? throw new ArgumentNullException(nameof(valueDeserializer));

            if (string.IsNullOrWhiteSpace(brokerList))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(brokerList));
            }

            if (string.IsNullOrWhiteSpace(groupId))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(groupId));
            }

            _consumerConfig = consumerConfig ?? ConsumerConfig.DefaultConfig;
        }

        public string BrokerList { get; protected set; }

        public string Topic { get; protected set; }
        public int GroupId { get; protected set; }
        public string ConsumerId { get; protected set; }
        public Dictionary<string, object> ConsumerConfiguration { get; set; }
        public ConcurrentDictionary<int, SlidingDoor> SlidingDoors { get; protected set; }

        public string Id => $"{GroupId}.{Topic}.{ConsumerId}";

        public void CommitOffset(IMessageContext messageContext)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _consumer = new Consumer<TKey, TValue>(ConsumerConfiguration, _keyDeserializer, _valueDeserializer);
            _consumer.Subscribe(Topic);
            _consumer.OnError += (sender, error) => _logger.Error($"consumer({Id}) error:{error.ToJson()}");
            _consumer.OnMessage += _consumer_OnMessage;
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        private void _consumer_OnMessage(object sender, Message<TKey, TValue> message)
        {
            try
            {
                AddMessage(message);
                _onMessageReceived(this, message);
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
                if (message.Value != null)
                {
                    FinishConsumingMessage(message.Partition, message.Offset);
                }
                _logger.Error(e.GetBaseException().Message, e);
            }
        }

        public void FinishConsumingMessage(int partition, long offset)
        {
            var slidingDoor = SlidingDoors.TryGetValue(partition);
            if (slidingDoor == null)
            {
                throw new Exception("partition slidingDoor not exists");
            }
            slidingDoor.RemoveOffset(offset);
        }

        protected void AddMessage(Message<TKey, TValue> message)
        {
            var slidingDoor = SlidingDoors.GetOrAdd(message.Partition, partition => new SlidingDoor(CommitOffset
                , string.Empty
                , partition
                , Configuration.Instance.GetCommitPerMessage()));
            slidingDoor.AddOffset(message.Offset);
        }

        public void CommitOffset(int partition, long offset)
        {
            //kafka not use broker in cluster node
            CommitOffset(null, partition, offset);
        }

        private void CommitOffset(string broker, int partition, long offset)
        {
            //kafka not use broker in cluster node
            CommitOffsetAsync(partition, offset);
        }

        public async Task CommitOffsetAsync(int partition, long offset)
        {
            var topicPartitionOffset = new TopicPartitionOffset(new TopicPartition(Topic, partition), offset + 1);
            var committedOffset = await _consumer.CommitAsync(new[] { topicPartitionOffset })
                .ConfigureAwait(false);
            if (committedOffset.Error.Code != ErrorCode.NoError)
            {
                _logger.Error($"{Id} committed offset failed {committedOffset.Error}");
            }
            else
            {
                _logger.DebugFormat($"{Id} committed offset {committedOffset.Offsets.FirstOrDefault()}");
            }
        }
    }
}