using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.DTOs.UserModels.FormModels
{
    public class ChangePasswordFormModel
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string oldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
