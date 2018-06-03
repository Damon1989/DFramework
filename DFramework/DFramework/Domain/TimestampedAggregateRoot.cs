using System.ComponentModel.DataAnnotations;

namespace DFramework.Domain
{
    public class TimestampedAggregateRoot : AggregateRoot
    {
        [Timestamp]
#if mysql
        public DateTime Version
#else

        public byte[] Version
#endif
        { get; private set; }
    }
}