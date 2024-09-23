namespace EmployMe.Models.Domain
{
    public class Application
    {
        public int EmployeeId { get; set; }
        public int JobId { get; set; }
        public DateTime ApplyDate { get; set; } = DateTime.UtcNow;
        public bool Accepted { get; set; } = false;

        // Navigation Properties
        public virtual Employee Employee { get; set; }
        public virtual AvailableJob Job { get; set; }
    }
}
