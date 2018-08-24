using System;
using System.ComponentModel.DataAnnotations;

namespace KafkaDemo
{
    public class KafkaProducerMessage
    {
        public int KafkaProducerMessageId { get; set; }

        [Required, StringLength(1000)]
        public string Topic { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}