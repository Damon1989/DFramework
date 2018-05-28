using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.Message
{
    public interface IMessageHandler
    {
        void Handle(object message);
    }

    public interface IMessageHandler<in TMessage> where TMessage : class
    {
        void Handler(TMessage message);
    }
}