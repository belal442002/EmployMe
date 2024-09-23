using System.ComponentModel.DataAnnotations;

namespace EmployMe.Models.DTO.ProfileDto
{
    public class UpdateProfileDto
    {
        public IFormFile? Image { get; set; }
    }
}
