using DFramework.Message;

namespace DFramework.Event
{
    public interface IEventSubscriber<in TEvent> :
        IMessageHandler<TEvent> where TEvent : class, IEvent
    {
    }
}