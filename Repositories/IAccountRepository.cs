using EmployMe.Models.Domain;
using EmployMe.Models.DTO;
using Microsoft.AspNetCore.Identity;

namespace EmployMe.Repositories
{
    public interface IAccountRepository
    {
        
        Task<List<Account>> GetAllAsync();
        Task<Account> GetByIdAsync(string id);

        Task<bool> CreateAsync(Account account, string password);
        Task<IdentityResult> UpdateAsync(string Id,Account account, string newPassword);
        Task<bool> DeleteAsync(string userId);   
        

       

    }
}
