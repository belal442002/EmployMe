using System.ComponentModel.DataAnnotations;

namespace EmployMe.Models.DTO.EmployeeDto
{
    public class UpdateEmployeeProfileDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "First Name cannot be over 50 over characters")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Last Name cannot be over 50 over characters")]
        public string LastName { get; set; }
        [Required]
        public IFormFile CV { get; set; }
        [Required]
        [EmailAddress]
        public string ContactEmail { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public IFormFile? Image { get; set; }
    }
}
