using System.Collections.Generic;
using System.Linq;
using DCommon;
using DFramework.Command;
using DFramework.Infrastructure;

namespace DFramework.Event.Impl
{
    public class EventBus : IEventBus
    {
        protected List<ICommand> CommandQueue;
        protected List<IEvent> EventQueue;
        protected List<object> SagaResultQueue;
        protected List<IEvent> ToPublishAnywayEventQueue;

        public EventBus()
        {
            EventQueue = new List<IEvent>();
            CommandQueue = new List<ICommand>();
            SagaResultQueue = new List<object>();
            ToPublishAnywayEventQueue = new List<IEvent>();
        }

        public void Dispose()
        {
        }

        public void Publish<TMessage>(TMessage @event) where TMessage : IEvent
        {
            if (@event != null)
            {
                EventQueue.Add(@event);
            }
        }

        public void Publish<TMessage>(IEnumerable<TMessage> events) where TMessage : IEvent
        {
            events.ForEach(Publish);
        }

        public void SendCommand(ICommand command)
        {
            CommandQueue.Add(command);
        }

        public void PublishAnyway(params IEvent[] events)
        {
            Publish(events.AsEnumerable());
            ToPublishAnywayEventQueue.AddRange(events);
        }

        public IEnumerable<ICommand> GetCommands()
        {
            return CommandQueue;
        }

        public IEnumerable<IEvent> GetEvents()
        {
            return EventQueue;
        }

        public IEnumerable<object> GetSagaResults()
        {
            return SagaResultQueue;
        }

        public IEnumerable<IEvent> GetToPublishAnywayMessages()
        {
            return ToPublishAnywayEventQueue;
        }

        public void FinishSaga(object sagaResult)
        {
            SagaResultQueue.Add(sagaResult);
        }

        public void ClearMessage()
        {
            SagaResultQueue.Clear();
            EventQueue.Clear();
            CommandQueue.Clear();
            ToPublishAnywayEventQueue.Clear();
        }
    }
}