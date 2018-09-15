using System;
using System.Collections.Generic;
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

        /// <summary>
        /// kafka辅助类构造方法
        /// </summary>
        /// <param name="sectionName">config中配置节点名称</param>
        /// <param name="isProducer"></param>
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

        public bool IsProducer => _isProducer;

        /// <summary>
        /// 发送消息到队列
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="acks"></param>
        /// <param name="timeout"></param>
        public void Pub(List<string> datas, short acks = 1, TimeSpan? timeout = default(TimeSpan?))
        {
            _producerHelper.Pub(_config.Topic, datas, acks, timeout);
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="onMsg"></param>
        public void Sub(Action<string> onMsg)
        {
            _consumerHelper.Sub(_config.Topic, onMsg);
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        public void UnSub()
        {
            _consumerHelper.UnSub();
        }
    }
}