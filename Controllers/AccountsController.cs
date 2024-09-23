using AutoMapper;
using EmployMe.Mappings;
using EmployMe.Models.Domain;
using EmployMe.Models.DTO.AccountDto;
using EmployMe.Models.DTO.CompanyDto;
using EmployMe.Models.DTO.EmployeeDto;
using EmployMe.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Transactions;

namespace EmployMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository accountRepository;
        IMapper mapper;

        public AccountsController(IAccountRepository accountRepository,IMapper mapper)
        {
            this.accountRepository = accountRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetALl()
        {
            var accounts = await accountRepository.GetAllAsync();
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(string id)
        {
            var account = await accountRepository.GetByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateAccDto updateAccDto)
        {

            var accountDomainModel = updateAccDto.AccountUpdate();

            var updatedAccount = await accountRepository.GetByIdAsync(id);
            if (updatedAccount == null)
            {
                return NotFound("Account not Found");
            }

            var updateResult = await accountRepository.UpdateAsync(id, accountDomainModel, updateAccDto.Password);

            if (!updateResult.Succeeded)
            {
                return BadRequest(updateResult.Errors); // Handle account update failure
            }

            return Ok("User Updated successfully.");
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string userId)
        {
            var result = await accountRepository.DeleteAsync(userId);
            if (result)
            {
                return Ok("User deleted successfully.");
            }
            else
            {
                return NotFound("User not found.");
            }
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] AddAccountDto addAccountDto)
        {
            var accountDomainModel = addAccountDto.AddAccount();
            var result = await accountRepository.CreateAsync(accountDomainModel,addAccountDto.Password);
            if (result)
            {
                return Ok("User created successfully.");
            }
            else
            {
                return BadRequest("Failed to create user.");
            }
        }
    }
}
