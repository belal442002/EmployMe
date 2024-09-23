using EmployMe.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployMe.Models.DTO.JobDto
{
    public class AddJobReqDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(500,ErrorMessage = "The description must be no longer than 500 characters.")]
        [MinLength(50, ErrorMessage = "The description must be at least 50 characters long.")]
        public string Description { get; set; }
        [Required]
        public string Qualifications { get; set; }
        [Required]
        public string Duties { get; set; }
        [Required]
        public int CompanyId { get; set; }
    }
}
