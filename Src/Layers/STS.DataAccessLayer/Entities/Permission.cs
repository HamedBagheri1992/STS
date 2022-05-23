using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.DataAccessLayer.Entities
{
    [Table("Permission")]
    public class Permission : BaseEntity
    {
        public long RoleId { get; set; }
        public long ApllicationId { get; set; }
        public string Title { get; set; }
        public string DisplayTitle { get; set; }


        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        [ForeignKey("ApplicationId")]
        public virtual Application Application { get; set; }

    }
}
