namespace EmployMe.Models.DTO.JobDto
{
    public class AvailableJobDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Qualifications { get; set; }
        public string Duties { get; set; }
        public virtual CompanyDto.CompanyDto Company { get; set; }
    }
}
