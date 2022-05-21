using STS.DTOs.ApplicationModels.FormModels;
using STS.DTOs.ApplicationModels.ViewModels;
using STS.DTOs.RoleModels.FormModels;
using STS.DTOs.RoleModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.Interfaces.Contracts
{
    public interface IApplicationService
    {       
        Task<ApplicationViewModel> GetAsync();
        Task<ApplicationViewModel> GetAsync(long application);
        Task<bool> IsTitleDuplicateAsync(string title);
        Task<long> AddAsync(AddApplicationFormModel addFormModel);
        Task UpdateAsync(UpdateApplicationFormModel updateFormModel);
        Task DeleteAsync(long id);

        Task<bool> IsExistAsync(long applicationId);
        Task<bool> IsTitleDuplicateAsync(long id, string title);        
        Task<bool> IsApplicationValidAsync(long id);        
    }
}
