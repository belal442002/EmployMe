using EmployMe.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployMe.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<Account> userManager;
        public AccountRepository(UserManager<Account> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<List<Account>> GetAllAsync()
        {
            return await userManager.Users.ToListAsync();
        }

        public async Task<Account> GetByIdAsync(string id)
        {
            return await userManager.FindByIdAsync(id);
        }

        public async Task<IdentityResult> UpdateAsync(string Id, Account account, string newPassword)
        {
            var existingAccount = await userManager.FindByIdAsync(Id);

            existingAccount.UserName = account.UserName;
            existingAccount.Email = account.Email;

            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                var returnUser = existingAccount;

                // Generate password reset token
                var code = await userManager.GeneratePasswordResetTokenAsync(returnUser);

                // Reset the password using the new password
                var resetPasswordResult = await userManager.ResetPasswordAsync(returnUser, code, newPassword);

                if (!resetPasswordResult.Succeeded)
                {
                    // Handle password reset failure
                    return resetPasswordResult;
                }
            }
            var updateResult = await userManager.UpdateAsync(existingAccount);
            return updateResult;
        }

        public async Task<bool> DeleteAsync(string userId)
        {
            var existingAccount = await userManager.FindByIdAsync(userId);
            if (existingAccount == null)
            {
                return false; // Account not found
            }

            var deleteResult = await userManager.DeleteAsync(existingAccount);
            return deleteResult.Succeeded;
        }

        public async Task<bool> CreateAsync(Account account, string password)
        {
            var user = new Account
            {
                UserName = account.UserName,
                Email = account.Email,
                Type = account.Type
            };

            var result = await userManager.CreateAsync(user, password);
            return result.Succeeded;
        }

    }
}

