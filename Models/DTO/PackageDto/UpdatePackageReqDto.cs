using System.ComponentModel.DataAnnotations;

namespace EmployMe.Models.DTO.PackageDto
{
    public class UpdatePackageReqDto
    {
        [Required]
        public String Name { get; set; }
        [Required]
        [MaxLength(500, ErrorMessage = "The description must be no longer than 500 characters.")]
        [MinLength(50, ErrorMessage = "The description must be at least 50 characters long.")]
        public String Description { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Max_Job_Interviews { get; set; }
        [Required]
        public int Max_Cv_Recommendation { get; set; }
        [Required]
        public int Max_Vacancies { get; set; }
    }
}
