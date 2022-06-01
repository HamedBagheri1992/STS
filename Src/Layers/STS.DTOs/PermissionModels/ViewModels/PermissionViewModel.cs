using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.DTOs.PermissionModels.ViewModels
{
    public class PermissionViewModel
    {
        public long Id { get; set; }
        public long ApplicationId { get; set; }
        public string Title { get; set; }
        public string DisplayTitle { get; set; }
    }
}
