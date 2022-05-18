using STS.DTOs.PermissionModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.DTOs.RoleModels.ViewModels
{
    public class RoleViewModel
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public long ApplicationId { get; set; }
        public string ApplicationTitle { get; set; }
        public IEnumerable<PermissionViewModel> Permissions { get; set; }
    }
}
