using STS.DTOs.ApplicationModels.ViewModels;
using STS.DTOs.ResultModels;
using STS.DTOs.UserModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.Interfaces.Contracts
{
    public interface IUserService
    {
        Task<PaginatedResult<UserViewModel>> GetAsync(PaginationParam pagination);
        Task<UserViewModel> GetAsync(long id);
    }
}
