using AutoMapper;
using EmployMe.FileUploadService;
using EmployMe.Mappings;
using EmployMe.Models.Domain;
using EmployMe.Models.DTO.AccountDto;
using EmployMe.Models.DTO.CompanyDto;
using EmployMe.Models.DTO.EmailDto;
using EmployMe.Models.DTO.EmployeeDto;
using EmployMe.Models.DTO.ProfileDto;
using EmployMe.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using System.Transactions;

namespace EmployMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthCompaniesController : ControllerBase
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IProfileRepository profileRepository;
        private readonly IMapper mapper;
        private readonly UserManager<Account> userManager;
        private readonly ITokenRepository tokenRepository;
        private readonly FileService fileService;
        private readonly IEmailRepository emailRepository;

        public AuthCompaniesController(ICompanyRepository companyRepository, IProfileRepository profileRepository, IMapper mapper, UserManager<Account> userManager, ITokenRepository tokenRepository, FileService fileService, IEmailRepository emailRepository)
        {
            this.companyRepository = companyRepository;
            this.mapper = mapper;
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.fileService = fileService;
            this.emailRepository = emailRepository;
            this.profileRepository = profileRepository;
        }
        
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterCompanyDto registerCompanyDto)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var companyDomainModel = registerCompanyDto.CompanyRegister();
                    companyDomainModel.CommercialRegisterUrl = fileService.UploadFile("CommercialRegister", registerCompanyDto.CommercialRegister);
                    companyDomainModel.TaxIDUrl = fileService.UploadFile("TaxID", registerCompanyDto.TaxID);

                    companyDomainModel.active = false;
                    companyDomainModel.Available_CV_Recommendations = 0;
                    companyDomainModel.Available_Job_Interviews = 0;
                    companyDomainModel.Max_Vacancies = 0;

                    var account = new Account
                    {
                        UserName = registerCompanyDto.Email,
                        Email = registerCompanyDto.Email,
                        Type = "Company"
                    };

                    var registerResult = await userManager.CreateAsync(account, registerCompanyDto.Password);
                    if (registerResult.Succeeded)
                    {
                        registerResult = await userManager.AddToRoleAsync(account, "Company");
                        if (registerResult.Succeeded)
                        {
                            var profileModel = new Models.Domain.Profile
                            {
                                Type = "Company"
                            };
                            profileModel = await profileRepository.CreateAsync(profileModel);
                            companyDomainModel.AccountId = account.Id;
                            companyDomainModel.ProfileId = profileModel.Id;
                            companyDomainModel = await companyRepository.CreateAsync(companyDomainModel);

                            EmailDto emailDto = new EmailDto
                            {
                                To = registerCompanyDto.Email,
                                Subject = "Company was registered",
                                Body = "Company was registered! Wait for Admin's Approved."
                            };
                            emailRepository.SendEmail(emailDto);

                            transactionScope.Complete(); // Commit transaction
                            return Ok("Company was registered! Wait for Admin's Approval.");
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
                    if (roles != null && roles[0] == "Company")
                    {
                        var companyDomain = await companyRepository.GetByAccountIdAsync(user.Id);

                        if (companyDomain == null)
                        {
                             return StatusCode(401, "Email or password incorrect"); //unauthorized
                        }

                        if (companyDomain != null)
                        {
                            if (!companyDomain.active)
                            {
                                return StatusCode(202,"Admin hasn't approved yet"); //accepted
                            }
                        }
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




