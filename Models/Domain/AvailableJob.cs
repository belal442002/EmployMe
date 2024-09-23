using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployMe.Models.Domain
{
    [Table("AvailableJob")]
    public class AvailableJob
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        //---------------------------------------
        public string Description { get; set; }
        public string Qualifications { get; set; }
        public string Duties { get; set; }
        [ForeignKey(nameof(Company))]
        public int CompanyId { get; set; }

        // Navigation property
        public virtual Company Company { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
    }
}
