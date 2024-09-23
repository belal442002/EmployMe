using EmployMe.Models.Domain;
using EmployMe.Models.DTO.CompanyDto;

namespace EmployMe.Repositories
{
    public interface ICompanyRepository
    {
        
        Task<List<Company>> GetAllAsync();
        Task<List<Company>> GetAllActiveAsync();
        Task<List<Company>> GetAllInactiveAsync();
        Task<List<Company>> GetAllInfoAsync();
        Task<Company?> GetByIdAsync(int id);
        Task<Company?> GetByIdInfoAsync(int id);
        Task<Company?> GetByAccountIdAsync(string id);
        Task<Company> CreateAsync(Company company);
        Task<Company?> UpdateAsync(int id, Company company);
        Task<Company?> UpdateCVRecommendationsAsync(int id, Company company);
        Task<Company?> UpdateAvailableJobInterviewsAsync(int id, Company company);
        Task<Company?> UpdateCVRecommendations_JobInterviewsAsync(int id, Company company);
        Task<Company?> UpdatePackage(int id, Package package);
        Task<Company?> ApproveCompanyAsync(int id);
        Task<Company?> CheckRejectCompanyAsync(int id);
        Task<Company?> DeleteAsync(int id);

    }
}
