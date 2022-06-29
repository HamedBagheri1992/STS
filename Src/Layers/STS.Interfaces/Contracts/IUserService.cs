using STS.DTOs.ApplicationModels.ViewModels;
using STS.DTOs.ResultModels;
using STS.DTOs.UserModels.FormModels;
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
        Task<UserViewModel?> GetAsync(long id);
        Task<long> AddAsync(AddUserFormModel addFormModel);
        Task UpdateAsync(UpdateUserFormModel updateFormModel);        
        Task ChangePasswordAsync(ChangePasswordFormModel changePasswordFormModel);


        Task<bool> IsExistAsync(long id);
        Task<bool> IsUserNameDuplicateAsync(string userName);
        Task<bool> IsUserNameDuplicateAsync(long id, string userName);       
        Task<bool> IsPasswordValidAsync(long id, string oldPassword);
    }
}
