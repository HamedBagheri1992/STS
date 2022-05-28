using Microsoft.EntityFrameworkCore;
using Moq;
using STS.DataAccessLayer;
using STS.DataAccessLayer.Entities;
using STS.DTOs.ResultModels;
using STS.Tests.Helpers;
using STS.Tests.MockDatas;
using System.Linq;
using Xunit;

namespace STS.Tests.Systems.Services
{
    public class UserServiceTests
    {
        private Mock<DbSet<User>> _mockUserSet;
        private Mock<STSDbContext> _mockContext;

        public UserServiceTests()
        {
            _mockUserSet = new Mock<DbSet<User>>();
            _mockContext = new Mock<STSDbContext>();
        }
    }
}
