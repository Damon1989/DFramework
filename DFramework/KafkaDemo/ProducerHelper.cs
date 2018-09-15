using System;
using System.Collections.Generic;
using System.Linq;
using KafkaNet;
using KafkaNet.Protocol;

namespace KafkaDemo
{
    /// <summary>
    /// 生产者辅助类
    /// </summary>
    internal class ProducerHelper : IDisposable
    {
        private readonly Producer _producer;
        private BrokerHelper _brokerHelper;

        public ProducerHelper(BrokerHelper brokerHelper)
        {
            _brokerHelper = brokerHelper;
            _producer = new Producer(_brokerHelper.GetBroker());
        }

        /// <summary>
        /// 发送消息到队列
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="datas"></param>
        /// <param name="acks"></param>
        /// <param name="timeout"></param>
        /// <param name="codec"></param>
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