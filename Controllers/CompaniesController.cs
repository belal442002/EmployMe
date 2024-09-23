using AutoMapper;
using EmployMe.Models.Domain;
using EmployMe.Models.DTO;
using EmployMe.Models.DTO.CompanyDto;
using EmployMe.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployMe.Mappings;
using EmployMe.Models.DTO.ProfileDto;
using EmployMe.Models.DTO.AccountDto;
using EmployMe.FileUploadService;
using EmployMe.Models.DTO.EmailDto;
using System.Transactions;
using EmployMe.Models.DTO.EmployeeDto;

namespace EmployMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        
        private readonly ICompanyRepository companyRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IProfileRepository profileRepository;
        private readonly IPackageRepository packageRepository;
        private readonly FileService fileService;
        private readonly IEmailRepository emailRepository;
        private readonly IMapper mapper;

        public CompaniesController(ICompanyRepository companyRepository,IAccountRepository accountRepository, IMapper mapper, IProfileRepository profileRepository,IPackageRepository packageRepository, FileService fileService, IEmailRepository emailRepository)
        {
            this.companyRepository = companyRepository;
            this.accountRepository = accountRepository;
            this.mapper = mapper;
            this.profileRepository = profileRepository;
            this.packageRepository = packageRepository;
            this.fileService = fileService;
            this.emailRepository = emailRepository;
        }

        // companies info - foreign key
        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetAll()
        {
            var companiesDomain = await companyRepository.GetAllAsync();
            var companyDto = mapper.Map <List<CompanyDto>>(companiesDomain);   //dist-src
            return Ok(companyDto);
        }

        // companies info - image
        [HttpGet]
        [Route("ActiveInfo")]
        //[Authorize]
        public async Task<IActionResult> GetAllInfoActive()
        {
            var companiesDomain = await companyRepository.GetAllActiveAsync();
            var companyDto = companiesDomain.CompaniesActiveInfo();   //dist-src
            return Ok(companyDto);
        }

        // companies not all info - image
        [HttpGet]
        [Route("Active")]
        //[Authorize]
        public async Task<IActionResult> GetAllActive()
        {
            var companiesDomain = await companyRepository.GetAllActiveAsync();
            var companyDto = companiesDomain.CompaniesActive();   //dist-src
            return Ok(companyDto);
        }

        // companies register info - email account
        [HttpGet]
        [Route("Admin/Active")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllActiveAdmin()
        {
            var companiesDomain = await companyRepository.GetAllActiveAsync();
            var companyDto = companiesDomain.CompaniesAdminInfo();   //dist-src
            return Ok(companyDto);
        }

        // companies register info - email account
        [HttpGet]
        [Route("Admin/Inactive")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllInactiveAdmin()
        {
            var companiesDomain = await companyRepository.GetAllInactiveAsync();
            var companyDto = companiesDomain.CompaniesAdminInfo();   //dist-src
            return Ok(companyDto);
        }

        // companies info - image - email account
        [HttpGet]
        [Route("Info")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllInfo()
        {
            var companiesDomain = await companyRepository.GetAllInfoAsync();
            var companyDto = companiesDomain.CompaniesInfo();
            return Ok(companyDto);
        }

        // company info - foreign key
        [HttpGet]
        //[Authorize]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var companyDomain = await companyRepository.GetByIdAsync(id);
            if (companyDomain == null) { return NotFound(); }

            var companyDto = mapper.Map<CompanyDto>(companyDomain); //dist then src
            return Ok(companyDomain);
        }

        // company info - image - email account
        [HttpGet]
        //[Authorize(Roles = "Admin,Company")]
        [Route("Info/{id:int}")]
        public async Task<IActionResult> GetByIdInfo([FromRoute] int id)
        {
            var companyDomain = await companyRepository.GetByIdInfoAsync(id);
            if (companyDomain == null) { return NotFound(); }

            var companyDto = companyDomain.CompanyInfo();
            return Ok(companyDto);
        }

        // company not all info - image
        [HttpGet]
        //[Authorize(Roles = "Admin,Company")]
        [Route("Information/{id:int}")]
        public async Task<IActionResult> GetByIdInfoformation([FromRoute] int id)
        {
            var companyDomain = await companyRepository.GetByIdInfoAsync(id);
            if (companyDomain == null) { return NotFound(); }

            var companyDto = companyDomain.CompanyInformation();
            return Ok(companyDto);
        }

        // update comapny info
        [HttpPut]
        //[Authorize(Roles = "Company")]
        [Route("Update/{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] UpdateCompanyDto updateCompanyDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //map/convert DTO to domain model
            var companyModel = mapper.Map<Company>(updateCompanyDto);

            //update database
            companyModel = await companyRepository.UpdateAsync(id, companyModel);
            if (companyModel == null) { return NotFound(); }

            //convert Domain Model to DTO to pass it to client
            var companyDto = mapper.Map<CompanyDto>(companyModel);
            return Ok(companyDto);
        }

        // update comapny info - image - account
        [HttpPut]
        //[Authorize(Roles = "Company")]
        [Route("UpdateAll/{id:int}")]
        public async Task<IActionResult> UpdateAllInfo([FromRoute] int id, [FromForm] UpdateCompanyInfoDto updateCompanyInfoDto)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        // Map/convert DTO to domain model
                        var companyModel = updateCompanyInfoDto.CompanyUpdate();

                        // Update CVurl parameters
                        var old = await companyRepository.GetByIdInfoAsync(id);

                        // Update database information
                        var updatedCompany = await companyRepository.UpdateAsync(id, companyModel);
                        if (updatedCompany == null)
                        {
                            return NotFound("Company not found"); // Company not found
                        }

                        // Update profile information
                        var updateProfileDto = new CreateProfileDto
                        {
                            Image = updateCompanyInfoDto.Image,
                            Type = "Company"
                        };
                        var profileModel = updateProfileDto.ProfileCreate(); //update type
                        profileModel.ImageUrl = fileService.UpdateFile("ProfileImages", updateCompanyInfoDto.Image, old.Profile.ImageUrl); //update image,solved
                        profileModel = await profileRepository.UpdateAsync(updatedCompany.ProfileId, profileModel);

                        // Update account information
                        var accountDomainModel = new Account
                        {
                            UserName = updateCompanyInfoDto.Email,
                            Email = updateCompanyInfoDto.Email
                        };
                        var updatedAccount = await accountRepository.GetByIdAsync(updatedCompany.AccountId);
                        if (updatedAccount == null)
                        {
                            return NotFound("Account not Found");
                        }

                        var updateResult = await accountRepository.UpdateAsync(updatedCompany.AccountId, accountDomainModel, updateCompanyInfoDto.Password);

                        if (!updateResult.Succeeded)
                        {
                            transactionScope.Dispose(); // Rollback transaction
                            return BadRequest(updateResult.Errors); // Handle account update failure
                        }

                        transactionScope.Complete(); // Commit transaction
                        return Ok("Company Updated Successfully");
                    }
                    else
                    {
                        return BadRequest(ModelState);
                    }
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose(); // Rollback transaction
                    return StatusCode(500, "An error occurred while processing your request.");
                }
            }
        }

        // update comapny info - account
        [HttpPut]
        //[Authorize(Roles = "Company")]
        [Route("UpdateAll2/{id:int}")]
        public async Task<IActionResult> UpdateAllInfo2([FromRoute] int id, [FromForm] UpdateCompanyInfo2Dto updateCompanyInfo2Dto)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        // Map/convert DTO to domain model
                        var companyModel = updateCompanyInfo2Dto.CompanyUpdate2();

                        // Update CVurl parameters
                        var old = await companyRepository.GetByIdInfoAsync(id);

                        // Update database information
                        var updatedCompany = await companyRepository.UpdateAsync(id, companyModel);
                        if (updatedCompany == null)
                        {
                            return NotFound("Company not found"); // Company not found
                        }

                        // Update account information
                        var accountDomainModel = new Account
                        {
                            UserName = updateCompanyInfo2Dto.Email,
                            Email = updateCompanyInfo2Dto.Email
                        };
                        var updatedAccount = await accountRepository.GetByIdAsync(updatedCompany.AccountId);
                        if (updatedAccount == null)
                        {
                            return NotFound("Account not Found");
                        }

                        var updateResult = await accountRepository.UpdateAsync(updatedCompany.AccountId, accountDomainModel, updateCompanyInfo2Dto.Password);

                        if (!updateResult.Succeeded)
                        {
                            transactionScope.Dispose(); // Rollback transaction
                            return BadRequest(updateResult.Errors); // Handle account update failure
                        }

                        transactionScope.Complete(); // Commit transaction
                        return Ok("Company Updated Successfully");
                    }
                    else
                    {
                        return BadRequest(ModelState);
                    }
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose(); // Rollback transaction
                    return StatusCode(500, "An error occurred while processing your request.");
                }
            }
        }

        // update company info - image
        [HttpPut]
        //[Authorize(Roles = "Company")]
        [Route("UpdateAllCompanyProfile/{id:int}")]
        public async Task<IActionResult> UpdateAllCompanyProfileInfo([FromRoute] int id, [FromForm] UpdateCompanyProfileDto updateCompanyProfileDto)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        // Map/convert DTO to domain model
                        var companyModel = updateCompanyProfileDto.CompanyProfileUpdate();

                        // Update CVurl parameters
                        var old = await companyRepository.GetByIdInfoAsync(id);

                        // Update database information
                        var updatedCompany = await companyRepository.UpdateAsync(id, companyModel);
                        if (updatedCompany == null)
                        {
                            return NotFound(); // Company not found
                        }

                        // Update profile information
                        var updateProfileDto = new CreateProfileDto
                        {
                            Image = updateCompanyProfileDto.Image,
                            Type = "Company"
                        };
                        var profileModel = updateProfileDto.ProfileCreate(); //update type
                        profileModel.ImageUrl = fileService.UpdateFile("ProfileImages", updateCompanyProfileDto.Image, old.Profile.ImageUrl); //update image,solved
                        profileModel = await profileRepository.UpdateAsync(updatedCompany.ProfileId, profileModel);

                        transactionScope.Complete(); // Commit transaction
                        return Ok("Company Updated Successfully");
                    }
                    else
                    {
                        return BadRequest(ModelState);
                    }
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose(); // Rollback transaction
                    return StatusCode(500, "An error occurred while processing your request.");
                }
            }
        }

        // update company image
        [HttpPut]
        [Route("UpdateCompanyProfile/{id:int}")]
        //[Authorize]
        public async Task<IActionResult> UpdateCompanyProfile([FromRoute] int id, [FromForm] UpdateProfileDto updateProfileDto)
        {
            if (ModelState.IsValid)
            {
                //Update CVurl parameters
                var old = await companyRepository.GetByIdInfoAsync(id);
                if (old == null)
                {
                    return NotFound(); // Company not found
                }

                var profileModel = new Models.Domain.Profile(); //update type
                profileModel.ImageUrl = fileService.UpdateFile("ProfileImages", updateProfileDto.Image, old.Profile.ImageUrl); //update image,solved
                profileModel = await profileRepository.UpdateAsync(old.ProfileId, profileModel);

                return Ok("Company Image Updated Successfully");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // update company account
        [HttpPut]
        //[Authorize(Roles = "Company")]
        [Route("UpdateCompanyAccount/{id:int}")]
        public async Task<IActionResult> UpdateCompanyAccount([FromRoute] int id, [FromBody] UpdateAccountDto updateAccountDto)
        {
            if (ModelState.IsValid)
            {
                var companyDomain = await companyRepository.GetByIdAsync(id);
                if (companyDomain == null)
                {
                    return NotFound(); // Company not found
                }

                // Update account information
                var accountDomainModel = new Account
                {
                    UserName = updateAccountDto.Email,
                    Email = updateAccountDto.Email
                };
                var updatedAccount = await accountRepository.GetByIdAsync(companyDomain.AccountId);
                if (updatedAccount == null)
                {
                    return NotFound("Account not Found");
                }

                var updateResult = await accountRepository.UpdateAsync(companyDomain.AccountId, accountDomainModel, updateAccountDto.Password);

                if (!updateResult.Succeeded)
                {
                    return BadRequest(updateResult.Errors); // Handle account update failure
                }

                return Ok("Company Updated Successfully");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        
        [HttpPut]
        //[Authorize(Roles = "Admin")]
        [Route("UpdateCVRecommendations/{id:int}")]
        public async Task<IActionResult> UpdateCVRecommendations([FromRoute] int id, [FromBody] UpdateCompanyCvRDto updateCompanyCvRDto)
        {
            if (ModelState.IsValid)
            {
                //map convert DTO to domain model
                var companyDomainModel = mapper.Map<Company>(updateCompanyCvRDto);

                companyDomainModel = await companyRepository.UpdateCVRecommendationsAsync(id, companyDomainModel);
                if (companyDomainModel == null) { return NotFound(); }

                //convert Domain Model to DTO to pass it to client
                var companyDto = mapper.Map<CompanyDto>(companyDomainModel);
                return Ok(companyDto);
            }
            else { return BadRequest(ModelState); }
        }
        
        [HttpPut]
        //[Authorize(Roles = "Admin")]
        [Route("UpdateAvailableJobInterviews/{id:int}")]
        public async Task<IActionResult> UpdateAvailableJobInterviews([FromRoute] int id, [FromBody] UpdateCompanyJobInterviewsDto updateCompanyJobInterviewsDto)
        {
            if (ModelState.IsValid)
            {
                //map convert DTO to domain model
                var companyDomainModel = mapper.Map<Company>(updateCompanyJobInterviewsDto);

                companyDomainModel = await companyRepository.UpdateAvailableJobInterviewsAsync(id, companyDomainModel);
                if (companyDomainModel == null) { return NotFound(); }

                //convert Domain Model to DTO to pass it to client
                var companyDto = mapper.Map<CompanyDto>(companyDomainModel);
                return Ok(companyDto);
            }
            else { return BadRequest(ModelState); }
        }
        
        [HttpPut]
        //[Authorize(Roles = "Admin")]
        [Route("UpdateCVRecommendations_JobInterviews/{id:int}")]
        public async Task<IActionResult> UpdateCVRecommendations_JobInterviews([FromRoute] int id, [FromBody] UpdateCompanyCvRnadInterviewsDto updateCompanyCvRnadInterviewsDto)
        {
            if (ModelState.IsValid)
            {
                //map convert DTO to domain model
                var companyDomainModel = mapper.Map<Company>(updateCompanyCvRnadInterviewsDto);

                companyDomainModel = await companyRepository.UpdateCVRecommendations_JobInterviewsAsync(id, companyDomainModel);
                if (companyDomainModel == null) { return NotFound(); }

                //convert Domain Model to DTO to pass it to client
                var companyDto = mapper.Map<CompanyDto>(companyDomainModel);
                return Ok(companyDto);
            }
            else { return BadRequest(ModelState); }
        }

        [HttpPut]
        //[Authorize(Roles = "Company")]
        [Route("UpdatePackage/{id:int}")]
        public async Task<IActionResult> UpdatePackage([FromRoute] int id, [FromBody] CompanyPackageDto companyPackageDto)
        {
            if (ModelState.IsValid)
            {
                var packageModel = await packageRepository.GetPackageAsync(companyPackageDto.PackageId);
                if (packageModel == null) { return NotFound("package not found"); }

                var companyDomainModel = await companyRepository.UpdatePackage(id, packageModel);
                if (companyDomainModel == null) { return NotFound("company not found"); }

                //convert Domain Model to DTO to pass it to client
                var companyDto = mapper.Map<CompanyDto>(companyDomainModel);
                return Ok(companyDto);
            }
            else { return BadRequest(ModelState); }
        }

        [HttpPut]
        //[Authorize(Roles = "Admin")]
        [Route("ApproveCompany/{id:int}")]
        public async Task<IActionResult> ApproveCompany([FromRoute] int id)
        {
            var companyDomainModel = await companyRepository.ApproveCompanyAsync(id);
            if (companyDomainModel == null) { return NotFound(); }

            // send email
            EmailDto emailDto = new EmailDto
            {
                To = companyDomainModel.Account.Email,
                Subject = "Company Approved",
                Body = "Company Approved"
            };
            emailRepository.SendEmail(emailDto);

            //convert Domain Model to DTO to pass it to client
            var companyDto = mapper.Map<CompanyDto>(companyDomainModel);
            return Ok(companyDto);
        }

        [HttpDelete]
        //[Authorize(Roles = "Admin")]
        [Route("RejectCompany/{id:int}")]
        public async Task<IActionResult> RejectCompany([FromRoute] int id)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var companyDomainModel = await companyRepository.CheckRejectCompanyAsync(id);
                    if (companyDomainModel == null) { return NotFound(); }

                    var companyModel = await companyRepository.DeleteAsync(id);
                    fileService.DeleteFile(companyModel.CommercialRegisterUrl); // delete Commercial Register from wwwroot
                    fileService.DeleteFile(companyModel.TaxIDUrl); // delete Tax ID from wwwroot

                    if (companyModel == null) { return NotFound(); }

                    var profile = await profileRepository.DeleteAsync(companyModel.ProfileId);
                    fileService.DeleteFile(profile.ImageUrl); // delete image from wwwroot

                    var result = await accountRepository.DeleteAsync(companyModel.AccountId);

                    if (!result)
                    {
                        transactionScope.Dispose(); // Rollback transaction
                        return BadRequest(ModelState);
                    }

                    EmailDto emailDto = new EmailDto
                    {
                        To = companyDomainModel.Account.Email,
                        Subject = "Company Rejected",
                        Body = "Company Rejected"
                    };
                    emailRepository.SendEmail(emailDto);

                    transactionScope.Complete(); // Commit transaction
                    return Ok("Company Rejected Successfully");
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose(); // Rollback transaction
                    return StatusCode(500, "An error occurred while processing your request.");
                }
            }
        }

        [HttpDelete]
        //[Authorize(Roles = "Company")]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var companyModel = await companyRepository.DeleteAsync(id);
                    fileService.DeleteFile(companyModel.CommercialRegisterUrl); // delete Commercial Register from wwwroot
                    fileService.DeleteFile(companyModel.TaxIDUrl); // delete Tax ID from wwwroot

                    if (companyModel == null) { return NotFound(); }

                    var profile = await profileRepository.DeleteAsync(companyModel.ProfileId);
                    fileService.DeleteFile(profile.ImageUrl); // delete image from wwwroot

                    var result = await accountRepository.DeleteAsync(companyModel.AccountId);

                    if (!result)
                    {
                        transactionScope.Dispose(); // Rollback transaction
                        return BadRequest(ModelState);
                    }

                    transactionScope.Complete(); // Commit transaction
                    return Ok("Company Deleted Successfully");
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose(); // Rollback transaction
                    return StatusCode(500, "An error occurred while processing your request.");
                }
            }
        }
    
    }
}
