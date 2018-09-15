using System;
using KafkaNet;
using KafkaNet.Model;

namespace KafkaDemo
{
    /// <summary>
    /// 代理辅助类
    /// </summary>
    internal class BrokerHelper
    {
        private readonly string _broker;

        public BrokerHelper(string broker)
        {
            _broker = broker;
        }

        /// <summary>
        ///  获取代理的路由对象
        /// </summary>
        /// <returns></returns>
        public BrokerRouter GetBroker()
        {
            var options = new KafkaOptions(new Uri($"http://{_broker}"));
            return new BrokerRouter(options);
        }
    }
}