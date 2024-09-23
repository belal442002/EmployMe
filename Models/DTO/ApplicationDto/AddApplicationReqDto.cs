using System.ComponentModel.DataAnnotations;

namespace EmployMe.Models.DTO.ApplicationDto
{
    public class AddApplicationReqDto
    {
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public int JobId { get; set; }
    }
}
