using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaDemo
{
    public class VwMaxOffsetByPartitionAndTopic
    {
        [Key, StringLength(1000), Column(Order = 0)]
        public string Topic { get; set; }

        [Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Partition { get; set; }

        public long? MaxOffset { get; set; }
    }
}