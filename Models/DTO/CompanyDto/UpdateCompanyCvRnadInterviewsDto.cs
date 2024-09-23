using System.ComponentModel.DataAnnotations;

namespace EmployMe.Models.DTO.CompanyDto
{
    public class UpdateCompanyCvRnadInterviewsDto
    {
        [Required]
        public int Available_CV_Recommendations { get; set; }
        [Required]
        public int Available_Job_Interviews { get; set; }
    }
}
