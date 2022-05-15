using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using STS.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.Services.Extensions
{
    public static class DbContextExtensions
    {
        public static void RegisterDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<STSDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }
    }
}
