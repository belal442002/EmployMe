using EmployMe.Models.Domain;
using EmployMe.Models.DTO.JobDto;

namespace EmployMe.Models.DTO.ApplicationDto
{
    public class ApplicationDto
    {
        public int EmployeeId { get; set; }
        public int JobId { get; set; }
        public DateTime ApplyDate { get; set; } = DateTime.UtcNow;
        public bool Accepted { get; set; }
        public virtual EmployeeDto.EmployeeDto Employee { get; set; }
        public virtual AvailableJobDto Job { get; set; }
    }
}
