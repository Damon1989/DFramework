namespace DFramework.Domain
{
    using System.Collections.Generic;
    using System.Linq;

    using DFramework.Event;
    using DFramework.Exceptions;

    public abstract class AggregateRoot : Entity, IAggregateRoot
    {
        private readonly Queue<IAggregateRootEvent> _eventQueue = new Queue<IAggregateRootEvent>();

        private string _aggregateRootType;

        private string AggregateRootName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this._aggregateRootType))
                {
                    var aggregateRootType = this.GetType();
                    if (this.GetType().Module.ToString() == "EntityProxyModule")
                        aggregateRootType = aggregateRootType.BaseType;
                    this._aggregateRootType = aggregateRootType.FullName;
                }

                return this._aggregateRootType;
            }
        }

        public IEnumerable<IAggregateRootEvent> GetDomainEvents()
        {
            return this._eventQueue.ToList();
        }

        public virtual void Rollback()
        {
            this._eventQueue.Clear();
        }

        protected virtual void OnEvent<TDomainEvent>(TDomainEvent @event)
            where TDomainEvent : class, IAggregateRootEvent
        {
            this.HandleEvent(@event);
            @event.AggregateRootName = this.AggregateRootName;
            this._eventQueue.Enqueue(@event);
        }

        protected virtual void OnException<TDomainException>(TDomainException exception)
            where TDomainException : IAggregateRootExceptionEvent
        {
            exception.AggregateRootName = this.AggregateRootName;
            throw new DomainException(exception);
        }

        private void HandleEvent<TDomainEvent>(TDomainEvent @event)
            where TDomainEvent : class, IAggregateRootEvent
        {
            var subscriber = this as IEventSubscriber<TDomainEvent>;
            subscriber?.Handle(@event);
        }
    }
}