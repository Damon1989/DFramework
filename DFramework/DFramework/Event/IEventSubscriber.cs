using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFramework.Event;

namespace DFramework.Message
{
    public interface IEventSubscriber<in TEvent> :
        IMessageHandler<TEvent> where TEvent : class, IEvent
    {
    }
}