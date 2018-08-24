using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KafkaNet;
using KafkaNet.Protocol;

namespace KafkaDemo
{
    internal class ProducerHelper : IDisposable
    {
        private readonly Producer _producer;
        private BrokerHelper _brokerHelper;

        public ProducerHelper(BrokerHelper brokerHelper)
        {
            _brokerHelper = brokerHelper;
            _producer = new Producer(_brokerHelper.GetBroker());
        }

        public void Pub(string topic, List<string> datas, short acks = 1, TimeSpan? timeout = default(TimeSpan?),
            MessageCodec codec = MessageCodec.CodecNone)
        {
            var msgs = datas.Select(item => new Message(item)).ToList();
            _producer.SendMessageAsync(topic, msgs, acks, timeout, codec);
        }

        public void Dispose()
        {
            _producer?.Dispose();
        }
    }
}