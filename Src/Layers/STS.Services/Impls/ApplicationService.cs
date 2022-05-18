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

        public Task<bool> IsExistAsync(long applicationId)
        {
            throw new NotImplementedException();
        }
    }
}
