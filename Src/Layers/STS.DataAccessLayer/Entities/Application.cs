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
            Roles = new List<Role>();
        }

        public string Title { get; set; }
        public Guid SecretKey { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }


        public virtual ICollection<Role> Roles { get; set; }
    }
}
