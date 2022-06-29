using Microsoft.EntityFrameworkCore;
using Moq;
using STS.DataAccessLayer;
using STS.DataAccessLayer.Entities;

namespace STS.Tests.Systems.Services
{
    public class OrganizationServiceTests
    {
        private Mock<DbSet<Organization>> _mockOrganizationSet;
        private Mock<STSDbContext> _mockContext;

        public OrganizationServiceTests()
        {
            _mockOrganizationSet = new Mock<DbSet<Organization>>();
            _mockContext = new Mock<STSDbContext>();
        }
    }
}
