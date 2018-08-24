using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace KafkaDemo
{
    public class KafkaModel : DbContext
    {
        public KafkaModel() : base("name=KafkaModel")
        {
        }

        public virtual DbSet<KafkaConsumerMessage> KafkaConsumerMessage { get; set; }
        public virtual DbSet<KafkaProducerMessage> KafkaProducerMessage { get; set; }
        public virtual DbSet<KafkaProducerMessageArchive> KafkaProducerMessageArchive { get; set; }
        public virtual DbSet<VwMaxOffsetByPartitionAndTopic> VwMaxOffsetByPartitionAndTopic { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KafkaConsumerMessage>().ToTable("KafkaConsumerMessage").Property(e => e.Topic)
                .IsUnicode(false);
            modelBuilder.Entity<KafkaProducerMessage>().ToTable("KafkaProducerMessage").Property(e => e.Topic)
                .IsUnicode(false);
            modelBuilder.Entity<KafkaProducerMessageArchive>().ToTable("KafkaProducerMessageArchive")
                .Property(e => e.Topic).IsUnicode(false);
            modelBuilder.Entity<VwMaxOffsetByPartitionAndTopic>().ToTable("VwMaxOffsetByPartitionAndTopic")
                .Property(e => e.Topic).IsUnicode(false);
        }
    }
}