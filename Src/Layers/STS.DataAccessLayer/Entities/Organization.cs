using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.DataAccessLayer.Entities
{
    [Table("Organization")]
    public class Organization : BaseEntity
    {
        public Organization()
        {
            Children = new List<Organization>();
            Applications = new List<Application>();
            Users = new List<User>();
        }

        public long? ParentId { get; set; }
        public string Title { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }

        [ForeignKey("ParentId")]
        public Organization Parent { get; set; }
        public ICollection<Organization> Children { get; set; }


        public ICollection<Application> Applications { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
