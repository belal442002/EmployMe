using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployMe.Models.Domain
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        public string? Description { get; set; }
        public string Name { get; set; }
        public string CommercialRegisterUrl { get; set; } //سجل تجاري 
        public string TaxIDUrl { get; set; } //رقم ضريبي
        public string ContactEmail { get; set; }
        public string HR_Email { get; set; }
        public int Available_Job_Interviews { get; set; }
        public int Available_CV_Recommendations { get; set; }
        public int Max_Vacancies { get; set; }
        public bool active { get; set; }

        [ForeignKey(nameof(Profile))]
        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        [ForeignKey(nameof(Account))]
        public string AccountId { get; set; }
        public virtual Account Account { get; set; }
        public virtual ICollection<AvailableJob> Jobs { get; set; }
        
        //------------------------------------------
        [ForeignKey(nameof(Package))]
        public int? PackageId { get; set; } // Nullable foreign key
        public virtual Package? Package { get; set; }

    }
}
