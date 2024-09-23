using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployMe.Models.Domain
{
    [Table("Package")]
    public class Package
    {
        [Key]
        public int Id { get; set; }
        public  String Name { get; set; }
        public String Description { get; set; }
        public double Price { get; set; }
        public int Max_Job_Interviews { get; set; }
        public int Max_Cv_Recommendation { get; set; }
        public int Max_Vacancies { get; set; }
        public bool Active_YN { get; set; } = true;
        public virtual ICollection<Company> Companies { get; set; }

    }
}
