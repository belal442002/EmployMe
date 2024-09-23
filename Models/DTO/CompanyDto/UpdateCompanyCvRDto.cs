using System.ComponentModel.DataAnnotations;

namespace EmployMe.Models.DTO.CompanyDto
{
    public class UpdateCompanyCvRDto
    {
        [Required]
        public int Available_CV_Recommendations { get; set; }
    }
}
