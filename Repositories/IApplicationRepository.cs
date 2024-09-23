using EmployMe.Models.Domain;

namespace EmployMe.Repositories
{
    public interface IApplicationRepository
    {
        Task<Application?> GetApplicationAsync(int empId, int jobId);
        Task<ICollection<Application>> GetAllApplicationsAsync(bool? getAccepted);
        Task<ICollection<Employee>?> GetApplicantsByJobAsync(int jobId);
        Task<ICollection<AvailableJob>?> GetJobsByEmployeeAsync(int employeeId);
        Task<ICollection<Application>?> GetCompanyApplicationsAsync(int companyId);
        Task<bool> CreateApplicationAsync(Application application);
        Task<bool> DeleteApplicationAsync(int empolyeeId, int jobId);
        Task<bool> AcceptApplicantAsync(int empId, int jobId);
    }
}
