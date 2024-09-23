namespace EmployMe.Models.DTO.CompanyDto
{
    public class CompanyAdminDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string CommercialRegisterUrl { get; set; }
        public string TaxIDUrl { get; set; }
        public string ContactEmail { get; set; }
        public string HR_Email { get; set; }
    }
}
