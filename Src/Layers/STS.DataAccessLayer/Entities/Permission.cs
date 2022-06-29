using System.ComponentModel.DataAnnotations.Schema;

namespace STS.DataAccessLayer.Entities
{
    [Table("Permission")]
    public class Permission : BaseEntity
    {
        public Permission()
        {
            Roles = new List<Role>();
        }

        public long ApplicationId { get; set; }
        public string Title { get; set; }
        public string DisplayTitle { get; set; }
        public long? CategoryId { get; set; }



        [ForeignKey("ApplicationId")]
        public virtual Application Application { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

    }
}
