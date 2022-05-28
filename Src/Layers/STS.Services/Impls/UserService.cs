using STS.DTOs.ApplicationModels.ViewModels;
using STS.DTOs.ResultModels;
using STS.DTOs.UserModels.ViewModels;
using STS.Interfaces.Contracts;

namespace STS.Services.Impls
{
    public class UserService : IUserService
    {
        public Task<PaginatedResult<UserViewModel>> GetAsync(PaginationParam pagination)
        {
            throw new NotImplementedException();
        }

        public Task<UserViewModel> GetAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}
