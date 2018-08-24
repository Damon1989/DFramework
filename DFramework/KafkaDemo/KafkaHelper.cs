using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KafkaNet.Protocol;

namespace KafkaDemo
{
    public sealed class KafkaHelper
    {
        private readonly KafkaConfig _config;

        private readonly ConsumerHelper _consumerHelper;

        private readonly bool _isProducer = true;

        private readonly ProducerHelper _producerHelper;
        private BrokerHelper _brokerHelper;

        public KafkaHelper(string sectionName, bool isProducer = true)
        {
            _isProducer = isProducer;
            _config = KafkaConfig.GetConfig(sectionName);
            _brokerHelper = new BrokerHelper(_config.Broker);

            if (isProducer)
            {
                _producerHelper = new ProducerHelper(_brokerHelper);
            }
            else
            {
                _consumerHelper = new ConsumerHelper(_brokerHelper);
            }
        }

        public bool IsProducer
        {
            get { return _isProducer; }
        }

        public void Pub(List<string> datas, short acks = 1, TimeSpan? timeout = default(TimeSpan?))
        {
            _producerHelper.Pub(_config.Topic, datas, acks, timeout, MessageCodec.CodecNone);
        }

        public void Sub(Action<string> onMsg)
        {
            _consumerHelper.Sub(_config.Topic, onMsg);
        }

        public void UnSub()
        {
            _consumerHelper.UnSub();
        }
    }
}