using EmployMe.Models.Domain;

namespace EmployMe.Repositories
{
    public interface IJobRepository
    {
        Task<AvailableJob?> GetJobAsync(int id);
        Task<ICollection<AvailableJob>> GetAllJobsAsync(String? query = null);
        //--------------------------------------
        Task<ICollection<AvailableJob>> GetAllJobsCompanyAsync(int CompanyId, String? query = null);
        Task<bool?> CreateJobAsync(AvailableJob job);
        Task<bool> UpdateJobAsync(int id, AvailableJob job);
        Task<bool> DeleteJobAsync(int id);
    }
}
