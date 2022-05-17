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
        //public STSDbContext(DbContextOptions<STSDbContext> options) : base(options)
        //{
        //}

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }

    }
}
