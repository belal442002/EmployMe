using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployMe.Models.Domain
{
    public class Employee 
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CVurl { get; set; }
        public string ContactEmail { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Location { get; set; }
        public string PhoneNumber { get; set; }

        [ForeignKey(nameof(Profile))]
        public int ProfileId { get; set; }

        [ForeignKey(nameof(Account))]
        public string AccountId { get; set; }

        // Navigation Properties
        public virtual Profile Profile { get; set; }
        public virtual Account Account { get; set; }
        public virtual ICollection<Application> Applications { get; set; }


    }
}
