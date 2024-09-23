using System.ComponentModel.DataAnnotations;

namespace EmployMe.Models.DTO.CompanyDto
{
    public class UpdateCompanyJobInterviewsDto
    {
        [Required]
        public int Available_Job_Interviews { get; set; }
    }
}
