using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.DTOs.RoleModels.FormModels
{
    public class AddRoleFormModel
    {
        [Required]
        public string Caption { get; set; }

        [Required]
        public long ApplicationId { get; set; }
    }
}
