using System.ComponentModel.DataAnnotations;

namespace EmployMe.Models.DTO.ProfileDto
{
    public class CreateProfileDto
    {
        public IFormFile? Image { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
