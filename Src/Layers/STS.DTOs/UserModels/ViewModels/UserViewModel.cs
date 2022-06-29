using STS.DTOs.ApplicationModels.ViewModels;
using STS.DTOs.RoleModels.ViewModels;

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
        public DateTime? ExpirationDate { get; set; }

        public DateTime? LastLogin { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public IEnumerable<long> OrganizationIds { get; set; }

        public IEnumerable<ApplicationViewModel> Applications { get; set; }
        public IEnumerable<RoleViewModel> Roles { get; set; }
    }
}
