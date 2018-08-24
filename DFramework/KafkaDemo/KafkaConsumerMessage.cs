using System;
using System.ComponentModel.DataAnnotations;

namespace KafkaDemo
{
    public class KafkaConsumerMessage
    {
        public int KafkaConsumerMessageId { get; set; }

        [Required, StringLength(1000)]
        public string Topic { get; set; }

        public int Partition { get; set; }
        public long Offset { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}