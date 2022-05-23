using Microsoft.EntityFrameworkCore;
using STS.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Application>(entity =>
            {
                entity.HasIndex(a => a.Title).IsUnique();
                entity.HasIndex(a => new { a.SecretKey, a.Id });
            });


            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasIndex(p => new { p.Title, p.ApllicationId }).IsUnique();
            });


            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(r => new { r.Caption, r.ApplicationId }).IsUnique();
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
