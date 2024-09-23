using EmployMe.Models.Domain;
using EmployMe.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EmployMe.Mappings;
using EmployMe.Models.DTO.AccountDto;

namespace EmployMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthAcountsController : ControllerBase
    {
        private readonly UserManager<Account> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthAcountsController(UserManager<Account> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] AddAccountDto addAccountDto)
        {
            var accountDomainModel = addAccountDto.AddAccount();

            var registerResult = await userManager.CreateAsync(accountDomainModel, addAccountDto.Password);
            if (registerResult.Succeeded)
            {
                registerResult = await userManager.AddToRoleAsync(accountDomainModel, addAccountDto.Type);
                if (registerResult.Succeeded)
                {
                    return Ok("User registered successfully.");
                }
            }
            return StatusCode(500, registerResult.Errors);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginDto.Password); //return bool flag idicate if the password valid for the user
                if (checkPasswordResult)
                {
                    //get roles for this user
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        //create token
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                        //map to Dto
                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };
                        return Ok(response);
                    }
                }
            }
            return StatusCode(401, "Email or password incorrect"); //unauthorized
        }
    }
}
