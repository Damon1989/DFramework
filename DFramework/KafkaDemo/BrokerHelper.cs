using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public BrokerRouter GetBroker()
        {
            var options = new KafkaOptions(new Uri($"http://{_broker}"));
            return new BrokerRouter(options);
        }
    }
}