using STS.DTOs.ApplicationModels.FormModels;
using STS.DTOs.ApplicationModels.ViewModels;
using STS.DTOs.RoleModels.ViewModels;
using STS.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.Services.Impls
{
    public class ApplicationService : IApplicationService
    {
        public Task<long> AddAsync(AddApplicationFormModel addFormModel)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationViewModel> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationViewModel> GetAsync(long application)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsApplicationValidAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsExistAsync(long applicationId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsTitleDuplicateAsync(string title)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsTitleDuplicateAsync(long id, string title)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UpdateApplicationFormModel updateFormModel)
        {
            throw new NotImplementedException();
        }
    }
}
