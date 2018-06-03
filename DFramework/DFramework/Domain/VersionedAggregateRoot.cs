using System.ComponentModel.DataAnnotations;

namespace DFramework.Domain
{
    public class VersionedAggregateRoot : AggregateRoot
    {
        private int _newVersion;

        private int NewVersion
        {
            get
            {
                if (_newVersion == 0)
                {
                    _newVersion = Version + 1;
                }
                return _newVersion;
            }
        }

        [ConcurrencyCheck]
        public int Version { get; set; }

        public override void Rollback()
        {
            _newVersion = 0;
            base.Rollback();
        }

        protected override void OnEvent<TDomainEvent>(TDomainEvent @event)
        {
            @event.Version = NewVersion;
            Version = NewVersion;
            base.OnEvent(@event);
        }

        protected override void OnException<TDomainException>(TDomainException exception)
        {
            exception.Version = Version;
            base.OnException(exception);
        }
    }
}