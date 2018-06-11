using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DFramework.Domain;

namespace DFramework.KendoUI.Domain
{
    public class Node : AggregateRoot
    {
        public Node()
        {
            Id = Guid.NewGuid().ToString().Replace("-", "");
            Name = "名称" + Id;
            Capacity = 0;
            FileSize = 0;
            FileCount = 0;
            Status = NodeStatus.InUsing;
        }

        public Node(string id,
            string name,
            string urlHost,
            string config,
            long capacity,
            long fileSize,
            long fileCount,
            string fullType,
            NodeStatus status
        )
        {
            Id = id;
            Name = name;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string UrlHost { get; set; }
        public string Config { get; set; }
        public long Capacity { get; set; }
        public long FileSize { get; set; }
        public long FileCount { get; set; }
        public string FullType { get; set; }
        public NodeStatus Status { get; set; }
    }
}