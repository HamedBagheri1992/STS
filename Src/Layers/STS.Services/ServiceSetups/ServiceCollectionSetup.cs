using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using STS.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.Services.Extensions
{
    public static class ServiceCollectionSetup
    {
        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<STSDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
    }
}
