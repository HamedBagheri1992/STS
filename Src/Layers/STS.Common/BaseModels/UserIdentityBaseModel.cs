
namespace STS.Common.BaseModels
{
    public class UserIdentityBaseModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public long ApplicationId { get; set; }
        public int ExpirationDuration { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public List<KeyValueModel> RolePairs { get; set; }
        public List<KeyValueModel> PermissionPairs { get; set; }
    }
}
