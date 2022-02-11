using ForumCustom.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ForumCustom.DAL.EF
{
    public sealed class ForumCustomContext : DbContext//, IForumCustomContext
    {
        public ForumCustomContext(DbContextOptions<ForumCustomContext> options) : base(options)
        {
            Database.EnsureCreated();
            Database.MigrateAsync();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Member> Members { get; set; }

        public DbSet<RoleUser> RoleUsers { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<Topic> Topics { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public override EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
        {
            return base.Entry(entity);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=helloappdb1;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*    modelBuilder.Entity<Role>(e =>
                {
                    e.HasOne(r => r.User).WithMany(t => t.Roles);
                });
            */
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Role>().HasData(new Role { Id = 1, Name = "Admin" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = 2, Name = "User" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = 3, Name = "Author" });
        }
    }
}