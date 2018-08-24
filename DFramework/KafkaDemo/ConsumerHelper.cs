using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KafkaNet;
using KafkaNet.Model;
using KafkaNet.Protocol;

namespace KafkaDemo
{
    internal class ConsumerHelper
    {
        private readonly BrokerHelper _brokerHelper;
        private Consumer _consumer;

        private bool _unSub;

        public ConsumerHelper(BrokerHelper brokerHelper)
        {
            _brokerHelper = brokerHelper;
        }

        public void Sub(string topic, Action<string> onMsg)
        {
            _unSub = false;

            var option = new ConsumerOptions(topic, _brokerHelper.GetBroker());
            _consumer = new Consumer(option);

            Task.Run(() =>
            {
                while (!_unSub)
                {
                    IEnumerable<Message> msgs = _consumer.Consume();
                    Parallel.ForEach(msgs, msg => onMsg(Encoding.UTF8.GetString(msg.Value)));
                }
            });
        }

        public void UnSub()
        {
            _unSub = true;
        }
    }
}