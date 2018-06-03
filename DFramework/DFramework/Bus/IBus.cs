using System;
using System.Collections.Generic;

namespace DFramework.Bus
{
    public interface IBus<Message> : IDisposable
    {
        void Publish<TMessage>(TMessage @event) where TMessage : Message;

        void Publish<TMessage>(IEnumerable<TMessage> events) where TMessage : Message;
    }
}