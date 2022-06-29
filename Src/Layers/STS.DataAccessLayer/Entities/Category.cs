using System.ComponentModel.DataAnnotations.Schema;

namespace STS.DataAccessLayer.Entities
{
    [Table("Category")]
    public class Category : BaseEntity
    {
        public Category()
        {
            Permissions = new List<Permission>();
        }

        public long ApplicationId { get; set; }
        public string Title { get; set; }


        [ForeignKey("ApplicationId")]
        public virtual Application Application { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
