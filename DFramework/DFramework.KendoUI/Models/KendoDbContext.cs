using System;
using System.Data.Entity;
using System.Security.AccessControl;
using DFramework.EntityFramework;
using DFramework.JsonNet;
using DFramework.KendoUI.Domain;

namespace DFramework.KendoUI.Models
{
    public class KendoDbContextCreateDatabaseIfModelChanges : DropCreateDatabaseIfModelChanges<KendoDbContext>
    {
        protected override void Seed(KendoDbContext context)
        {
            try
            {
                var department = new Department();
                department.Add("001", "部门1", "");
                context.Departments.Add(department);

                var subDepartment = new Department();
                subDepartment.Add("001001", "部门2", department.Id);
                context.Departments.Add(subDepartment);

                var user = new User();
                user.Add("姓名", department.Id);
                context.Users.Add(user);

                var region1 = new RegionVocabulary("001", "区域1", CommonStatus.Normal);
                region1.Add(region1.GetValue());
                var region2 = new RegionVocabulary("002", "区域2", CommonStatus.Normal);
                region2.Add(region2.GetValue());
                context.Vocabulary.Add(region1);
                context.Vocabulary.Add(region2);

                var asset = new AssetVocabulary("001", "台式机", 10);
                asset.Add(asset.GetValue());

                var subAsset = new AssetVocabulary("00101", "DELL台式机", 10);
                subAsset.Add(subAsset.GetValue(), asset.Id);
                context.Vocabulary.Add(asset);
                context.Vocabulary.Add(subAsset);

                context.SaveChanges();
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
        public DbSet<Department> Departments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vocabulary> Vocabulary { get; set; }
        public DbSet<Asset> Assets { get; set; }

#if DEBUG

        static KendoDbContext()
        {
            System.Data.Entity.Database.SetInitializer(new KendoDbContextCreateDatabaseIfModelChanges());
        }

#endif

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().ToTable("basic_Department");
            modelBuilder.Entity<User>().ToTable("basic_User");
            modelBuilder.Entity<Vocabulary>().ToTable("basic_Vocabulary");
            base.OnModelCreating(modelBuilder);
        }
    }
}