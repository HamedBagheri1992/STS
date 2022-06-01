using System.ComponentModel.DataAnnotations;

namespace STS.DTOs.AccountModels.FormModels
{
    public class LoginFormModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public long AppId { get; set; }

        [Required]
        public Guid SecretKey { get; set; }
    }
}
