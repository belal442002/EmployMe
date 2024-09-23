using System.ComponentModel.DataAnnotations;

namespace EmployMe.Models.DTO.CompanyDto
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string CommercialRegisterUrl { get; set; }
        public string TaxIDUrl { get; set; }
        public string ContactEmail { get; set; }
        public string HR_Email { get; set; }
        public int Available_Job_Interviews { get; set; }
        public int Available_CV_Recommendations { get; set; }
        public int Max_Vacancies { get; set; }
        public bool active { get; set; }
        public string AccountId { get; set; }
        public int ProfileId { get; set; }
        public int PackageId { get; set; }
    }
}
