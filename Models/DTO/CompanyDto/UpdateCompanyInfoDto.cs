﻿using System.ComponentModel.DataAnnotations;

namespace EmployMe.Models.DTO.CompanyDto
{
    public class UpdateCompanyInfoDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot be over 50 over characters")]
        public string Name { get; set; }
        [MaxLength(1000, ErrorMessage = "Description cannot be over 1000 over characters")]
        public string? Description { get; set; }
        [Required]
        [EmailAddress]
        public string ContactEmail { get; set; }
        [Required]
        [EmailAddress]
        public string HR_Email { get; set; }
        public IFormFile? Image { get; set; }
    }
}