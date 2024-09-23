using EmployMe.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace EmployMe.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(Account account, List<string> roles);
    }
}
