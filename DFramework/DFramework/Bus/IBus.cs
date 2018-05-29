using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.Bus
{
    public interface IBus<Message> : IDisposable
    {
        void Publish<TMessage>(TMessage @event) where TMessage : Message;

        void Publish<TMessage>(IEnumerable<TMessage> events) where TMessage : Message;
    }
}