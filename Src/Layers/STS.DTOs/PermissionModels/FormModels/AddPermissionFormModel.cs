using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.DTOs.PermissionModels.FormModels
{
    public class AddPermissionFormModel
    {
        public int RoleId { get; set; }
        public string Title { get; set; }
        public string DisplayTitle { get; set; }
    }
}
