
using System.ComponentModel.DataAnnotations;

namespace STS.DTOs.OrganizationModel.FormModels
{
    public class AddOrganizationFormModel
    {
        public long? ParentId { get; set; }
        
        [Required]
        public string Title { get; set; }

        public string Tag { get; set; }
        public string Description { get; set; }
    }
}
