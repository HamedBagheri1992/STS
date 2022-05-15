using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.DataAccessLayer.Entities
{
    public class Permission : BaseEntity
    {
        public long RoleId { get; set; }
        public string Title { get; set; }
        public string DisplayTitle { get; set; }


        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}
