using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.DTOs.OrganizationModel.ViewModels
{
    public class OrganizationViewModel
    {
        public long Id { get; set; }
        public long? ParentId { get; set; }
        public string Title { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
    }
}
