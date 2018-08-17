using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.Message
{
    public class TopicAttribute : Attribute
    {
        public TopicAttribute(string topic)
        {
            Topic = topic;
        }

        public string Topic { get; set; }
    }
}