namespace EmployMe.Models.DTO.PackageDto
{
    public class PackageDto
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public double Price { get; set; }
        public int Max_Job_Interviews { get; set; }
        public int Max_Cv_Recommendation { get; set; }
        public int Max_Vacancies { get; set; }
    }
}
