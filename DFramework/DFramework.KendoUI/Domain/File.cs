using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using DFramework.Domain;

namespace DFramework.KendoUI.Domain
{
    public class File : AggregateRoot
    {
        public string Id { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        public int ReferenceCount { get; set; }
        public string NodeId { get; set; }

        [ForeignKey("NodeId")]
        public virtual Node Node { get; set; }

        public DateTime CreateTime { get; set; }
    }
}