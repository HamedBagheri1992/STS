using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.DTOs.UserModels.FormModels
{
    public class AddUserFormModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }
        
        public string Password { get; set; }

        [Required]
        public bool IsActive { get; set; }
        
        public DateTime? ExpirationDate { get; set; }
    }
}
