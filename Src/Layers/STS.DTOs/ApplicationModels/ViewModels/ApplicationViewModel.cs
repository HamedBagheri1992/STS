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
        public int RoleCount { get; set; }
        public int PermissionCount { get; set; }
        public int ExpirationDuration { get; set; }
    }
}
