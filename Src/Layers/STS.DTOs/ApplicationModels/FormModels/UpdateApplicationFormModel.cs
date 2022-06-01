using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.DTOs.ApplicationModels.FormModels
{
    public class UpdateApplicationFormModel : AddApplicationFormModel
    {
        [Required]
        public long Id { get; set; }
    }
}
