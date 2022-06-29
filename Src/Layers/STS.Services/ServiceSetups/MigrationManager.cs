using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using STS.Common.Extensions;
using STS.DataAccessLayer;
using STS.DataAccessLayer.Entities;

namespace STS.Services.ServiceSetups
{
    public static class MigrationManager
    {
        public static IHost MigrationDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var _context = scope.ServiceProvider.GetRequiredService<STSDbContext>())
                {
                    try
                    {
                        _context.Database.Migrate();

                        if (!_context.Applications.Any(x => x.Title == "STS"))
                        {
                            var application = new Application { Title = "STS", SecretKey = Guid.NewGuid(), Description = "Seed by STS", CreatedDate = DateTime.Now };
                            _context.Applications.Add(application);

                            var permission = new Permission() { Title = "STS-Permission", DisplayTitle = "STS Permission", Application = application };
                            _context.Permissions.Add(permission);

                            var role = new Role { Caption = "Admin", Application = application, Permissions = new List<Permission>() { permission } };
                            _context.Roles.Add(role);

                            var user = new User
                            {
                                UserName = "Admin",
                                Password = "123".ToHashPassword(),
                                FirstName = "Admin",
                                LastName = "System",
                                IsActive = true,
                                CreatedDate = DateTime.Now,
                                ModifiedDate = DateTime.Now,
                                Applications = new List<Application>() { application },
                                Roles = new List<Role>() { role }
                            };
                            _context.Users.Add(user);

                            _context.SaveChanges();
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return host;
        }
    }
}
