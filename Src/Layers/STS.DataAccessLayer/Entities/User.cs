using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.DataAccessLayer.Entities
{
    [Table("User")]
    public class User : BaseEntity
    {
        public User()
        {
            Applications = new List<Application>();
            Roles = new List<Role>();
            Organizations = new List<Organization>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [NotMapped]
        public string FullName => $"{this.FirstName} {this.LastName}";
        public string UserName { get; set; }
        public string Password { get; set; }

        public bool IsActive { get; set; }
        public DateTime? ExpirationDate{ get; set; }

        public DateTime? LastLogin { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }


        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<Organization> Organizations { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
