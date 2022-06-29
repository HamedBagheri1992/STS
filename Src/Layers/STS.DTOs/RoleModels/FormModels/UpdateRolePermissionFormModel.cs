
namespace STS.DTOs.RoleModels.FormModels
{
    public class UpdateRolePermissionFormModel
    {
        public long RoleId { get; set; }
        public List<long> PermissionIds { get; set; }
    }
}
