using AutoMapper;
using EmployMe.Mappings;
using EmployMe.Models.Domain;
using EmployMe.Models.DTO.EmployeeDto;
using EmployMe.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EmployMe.Models.DTO.CompanyDto;
using EmployMe.Models.DTO.ProfileDto;
using EmployMe.Models.DTO.AccountDto;
using EmployMe.FileUploadService;
using EmployMe.Models.DTO.EmailDto;
using System.Transactions;

namespace EmployMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthEmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IProfileRepository profileRepository;
        private readonly IMapper mapper;
        private readonly UserManager<Account> userManager;
        private readonly ITokenRepository tokenRepository;
        private readonly FileService fileService;
        private readonly IEmailRepository emailRepository;

        public AuthEmployeesController(IEmployeeRepository employeeRepository, IProfileRepository profileRepository, IMapper mapper, UserManager<Account> userManager, ITokenRepository tokenRepository, FileService fileService, IEmailRepository emailRepository)
        {
            this.employeeRepository = employeeRepository;
            this.profileRepository = profileRepository;
            this.mapper = mapper;
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.fileService = fileService;
            this.emailRepository = emailRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterEmployeeDto registerEmployeeDto)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var employeeModel = registerEmployeeDto.EmployeeRegister();
                    employeeModel.CVurl = fileService.UploadFile("CVs", registerEmployeeDto.CV);

                    var account = new Account
                    {
                        UserName = registerEmployeeDto.Email,
                        Email = registerEmployeeDto.Email,
                        Type = "Employee"
                    };

                    var createProfileDto = new CreateProfileDto
                    {
                        Type = "Employee"
                    };

                    var registerResult = await userManager.CreateAsync(account, registerEmployeeDto.Password);
                    if (registerResult.Succeeded)
                    {
                        registerResult = await userManager.AddToRoleAsync(account, "Employee");
                        if (registerResult.Succeeded)
                        {
                            var profileModel = createProfileDto.ProfileCreate();
                            profileModel = await profileRepository.CreateAsync(profileModel);
                            employeeModel.AccountId = account.Id;
                            employeeModel.ProfileId = profileModel.Id;
                            employeeModel = await employeeRepository.CreateAsync(employeeModel);

                            // send email
                            EmailDto emailDto = new EmailDto
                            {
                                To = registerEmployeeDto.Email,
                                Subject = "Account created",
                                Body = "Account created Successfully"
                            };
                            emailRepository.SendEmail(emailDto);

                            transactionScope.Complete(); // Commit transaction
                            return Ok("Employee was registered! please login.");
                        }
                        else
                        {
                            return StatusCode(500, registerResult.Errors);
                        }
                    }
                    return StatusCode(500, registerResult.Errors);
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    transactionScope.Dispose(); // Rollback transaction
                    return StatusCode(500, "An error occurred while processing your request.");
                }
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginCompanyDto)
        {
            var user = await userManager.FindByEmailAsync(loginCompanyDto.Email);
            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginCompanyDto.Password); //return bool flag idicate if the password valid for the user
                if (checkPasswordResult)
                {
                    //get roles for this user
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null && roles[0] == "Employee")
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
