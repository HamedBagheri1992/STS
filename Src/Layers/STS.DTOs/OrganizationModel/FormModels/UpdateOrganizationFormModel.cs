using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.DTOs.OrganizationModel.FormModels
{
    public class UpdateOrganizationFormModel : AddOrganizationFormModel
    {
        public long Id { get; set; }
    }
}
