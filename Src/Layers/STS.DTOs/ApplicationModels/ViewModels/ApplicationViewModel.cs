using STS.DTOs.PermissionModels.ViewModels;
using STS.DTOs.RoleModels.ViewModels;

namespace STS.DTOs.ApplicationModels.ViewModels
{
    public class ApplicationViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        public IEnumerable<RoleViewModel> Roles { get; set; }
        public IEnumerable<PermissionViewModel> Permissions { get; set; }
    }
}
