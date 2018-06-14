using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFramework.KendoUI.Domain
{
    public class AggregateRoot : DFramework.Domain.AggregateRoot
    {
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }

        public AggregateRoot()
        {
            CreatedTime = ModifiedTime = DateTime.Now;
        }
    }
}