using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.DTOs.PermissionModels.FormModels
{
    public class AddPermissionFormModel
    {
        //[Required]
        public int RoleId { get; set; }

        [Required]
        public string Title { get; set; }

        //[Required]
        public string DisplayTitle { get; set; }
    }
}
