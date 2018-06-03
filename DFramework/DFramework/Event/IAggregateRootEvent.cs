namespace DFramework.Event
{
    public interface IAggregateRootEvent : IEvent
    {
        object AggregateRootID { get; }
        string AggregateRootName { get; set; }
        int Version { get; set; }
    }
}