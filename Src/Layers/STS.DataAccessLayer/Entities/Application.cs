using System.ComponentModel.DataAnnotations.Schema;

namespace STS.DataAccessLayer.Entities
{
    [Table("Application")]
    public class Application : BaseEntity
    {
        public Application()
        {
            Users = new List<User>();
            Roles = new List<Role>();
            Permissions = new List<Permission>();
            Organizations = new List<Organization>();
            Categories = new List<Category>();
        }

        public string Title { get; set; }
        public Guid SecretKey { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ExpirationDuration { get; set; }


        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
        public virtual ICollection<Organization> Organizations { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}
