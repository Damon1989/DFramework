using System;
using System.Data.Entity;
using DFramework.EntityFramework;
using DFramework.KendoUI.Domain;

namespace DFramework.KendoUI.Models
{
    public class KendoDbContextCreateDatabaseIfModelChanges : DropCreateDatabaseIfModelChanges<KendoDbContext>
    {
        protected override void Seed(KendoDbContext context)
        {
            try
            {
                // init database
                base.Seed(context);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    public class KendoDbContext : MSDbContext
    {
        public KendoDbContext()
        : base("name=KendoDbContext")
        {
        }

        public DbSet<File> Files { get; set; }
        public DbSet<Node> Nodes { get; set; }

#if DEBUG

        static KendoDbContext()
        {
            System.Data.Entity.Database.SetInitializer(new KendoDbContextCreateDatabaseIfModelChanges());
        }

#endif

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}