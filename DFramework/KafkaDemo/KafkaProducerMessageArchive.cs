using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaDemo
{
    public class KafkaProducerMessageArchive
    {
        public int KafkaProducerMessageArchiveId { get; set; }
        public int KafkaProducerMessageId { get; set; }

        [Required, StringLength(1000)]
        public string Topic { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime ArchivedTime { get; set; }
    }
}