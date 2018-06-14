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
                var department = new Department();
                department.Add("001", "部门1", "");
                context.Departments.Add(department);

                var subDepartment = new Department();
                subDepartment.Add("001001", "部门2", department.Id);
                context.Departments.Add(subDepartment);

                var user = new User();
                user.Add("姓名", department.Id);
                context.Users.Add(user);
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
            base.OnModelCreating(modelBuilder);
        }
    }
}