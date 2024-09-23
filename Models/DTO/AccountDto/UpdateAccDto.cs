using System.ComponentModel.DataAnnotations;

namespace EmployMe.Models.DTO.AccountDto
{
    public class UpdateAccDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
