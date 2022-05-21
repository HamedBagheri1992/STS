using STS.DTOs.RoleModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.DTOs.ApplicationModels.ViewModels
{
    public class ApplicationViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public Guid SecretKey { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<RoleViewModel> Roles { get; set; }
    }
}
