using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.Event
{
    public interface IAggregateRootEvent : IEvent
    {
        object AggregateRootID { get; }
        string AggregateRootName { get; set; }
        int Version { get; set; }
    }
}