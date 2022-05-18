using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.DataAccessLayer.Entities
{
    [Table("Role")]
    public class Role : BaseEntity
    {
        public Role()
        {
            Permissions = new List<Permission>();
        }

        public string Caption { get; set; }
        public long ApplicationId { get; set; }

        [ForeignKey("ApplicationId")]
        public virtual Application Application { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
