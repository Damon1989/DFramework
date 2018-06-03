using System.Collections.Generic;
using System.Linq;
using DFramework.Event;
using DFramework.Message;

namespace DFramework.Domain
{
    public abstract class AggregateRoot : Entity, IAggregateRoot
    {
        private string _aggregateRootType;
        private readonly Queue<IAggregateRootEvent> _eventQueue = new Queue<IAggregateRootEvent>();

        private string AggregateRootName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_aggregateRootType))
                {
                    var aggregateRootType = GetType();
                    if (GetType().Module.ToString() == "EntityProxyModule")
                    {
                        aggregateRootType = aggregateRootType.BaseType;
                    }
                    _aggregateRootType = aggregateRootType.FullName;
                }
                return _aggregateRootType;
            }
        }

        public IEnumerable<IAggregateRootEvent> GetDomainEvents()
        {
            return _eventQueue.ToList();
        }

        public virtual void Rollback()
        {
            _eventQueue.Clear();
        }

        protected virtual void OnEvent<TDomainEvent>(TDomainEvent @event)
            where TDomainEvent : class, IAggregateRootEvent
        {
            HandleEvent(@event);
            @event.AggregateRootName = AggregateRootName;
            _eventQueue.Enqueue(@event);
        }

        private void HandleEvent<TDomainEvent>(TDomainEvent @event) where TDomainEvent : class, IAggregateRootEvent
        {
            var subscriber = this as IEventSubscriber<TDomainEvent>;
            subscriber?.Handler(@event);
        }
    }
}