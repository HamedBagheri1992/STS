using Microsoft.EntityFrameworkCore;
using STS.DataAccessLayer.Entities;

namespace STS.DataAccessLayer
{
    public class STSDbContext : DbContext
    {
        public STSDbContext()
        {

        }

        public STSDbContext(DbContextOptions<STSDbContext> options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.UserName).IsUnique();
                entity.HasIndex(u => new { u.UserName, u.Password });
            });

            modelBuilder.Entity<Application>(entity =>
            {
                entity.HasIndex(a => a.Title).IsUnique();
                entity.HasIndex(a => new { a.SecretKey, a.Id });
                entity.HasMany(e => e.Permissions).WithOne(e => e.Application).HasForeignKey(e => e.ApplicationId).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(e => e.Roles).WithOne(e => e.Application).HasForeignKey(e => e.ApplicationId).OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasIndex(p => new { p.Title, p.ApplicationId }).IsUnique();
            });


            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(r => new { r.Caption, r.ApplicationId }).IsUnique();
                entity.HasMany(p => p.Permissions).WithMany(p => p.Roles).UsingEntity<Dictionary<string, object>>("RolePermission", j =>
                {
                    j.HasOne<Role>().WithMany().HasForeignKey("RolesId").HasConstraintName("FK_RolePermission_Roless_RolesId").OnDelete(DeleteBehavior.NoAction);
                    j.HasOne<Permission>().WithMany().HasForeignKey("PermissionsId").HasConstraintName("FK_RolePermission_Permissions_PermissionsId").OnDelete(DeleteBehavior.NoAction);
                });
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
