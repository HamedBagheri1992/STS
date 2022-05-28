using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        public string Title { get; set; }
        public Guid SecretKey { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }


        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
