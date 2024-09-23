using System.ComponentModel.DataAnnotations;

namespace EmployMe.Models.DTO.JobDto
{
    public class UpdateJobReqDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(500, ErrorMessage = "The description must be no longer than 500 characters.")]
        [MinLength(50, ErrorMessage = "The description must be at least 50 characters long.")]
        public string Description { get; set; }
        [Required]
        public string Qualifications { get; set; }
        [Required]
        public string Duties { get; set; }
    }
}
