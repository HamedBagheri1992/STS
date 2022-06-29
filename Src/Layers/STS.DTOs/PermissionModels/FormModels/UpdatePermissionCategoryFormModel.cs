using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.DTOs.PermissionModels.FormModels
{
    public class UpdatePermissionCategoryFormModel
    {
        [Required]
        public long CategoryId { get; set; }
        public List<long> PermissionIds { get; set; }
    }
}
