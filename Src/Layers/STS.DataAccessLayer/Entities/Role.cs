using System.ComponentModel.DataAnnotations.Schema;

namespace STS.DataAccessLayer.Entities
{
    [Table("Role")]
    public class Role : BaseEntity
    {
        public Role()
        {
            Users = new List<User>();
            Permissions = new List<Permission>();
        }

        public string Caption { get; set; }
        public long ApplicationId { get; set; }

        [ForeignKey("ApplicationId")]
        public virtual Application Application { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
