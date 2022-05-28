using STS.DTOs.ApplicationModels.ViewModels;
using STS.DTOs.RoleModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.DTOs.UserModels.ViewModels
{
    public class UserViewModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get; set; }
        public string UserName { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime? LastLogin { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }


        public List<ApplicationViewModel> Applications { get; set; }
        public List<RoleViewModel> Roles { get; set; }
    }
}
