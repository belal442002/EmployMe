using AutoMapper;
using EmployMe.FileUploadService;
using EmployMe.Mappings;
using EmployMe.Models.Domain;
using EmployMe.Models.DTO.AccountDto;
using EmployMe.Models.DTO.CompanyDto;
using EmployMe.Models.DTO.EmployeeDto;
using EmployMe.Models.DTO.ProfileDto;
using EmployMe.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EmployMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IMapper mapper;
        private readonly IProfileRepository profileRepository;
        private readonly FileService fileService;

        public EmployeesController(IEmployeeRepository employeeRepository, IAccountRepository accountRepository, IMapper mapper, IProfileRepository profileRepository, FileService fileService)
        {
            this.employeeRepository = employeeRepository;
            this.accountRepository = accountRepository;
            this.mapper = mapper;
            this.profileRepository = profileRepository;
            this.fileService = fileService;
        }

        // employees info - foreign key
        [HttpGet]
        //[Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetAll()
        {
            var employeesModel = await employeeRepository.GetAllAsync();
            var employeesDto = mapper.Map<List<EmployeeDto>>(employeesModel);   //dist-src
            return Ok(employeesDto);
        }

        // employees info
        [HttpGet]
        [Route("Information")]
        //[Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetAllEmployeeInformation()
        {
            var employeesModel = await employeeRepository.GetAllAsync();
            var employeesDto = mapper.Map<List<EmployeeInformationDto>>(employeesModel);   //dist-src
            return Ok(employeesDto);
        }

        // employees info - image
        [HttpGet]
        [Route("ProfileInfo")]
        //[Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetAllProfileInfo()
        {
            var employeesModel = await employeeRepository.GetAllInfoAsync();
            var employeesDto = employeesModel.EmployeesProfileInfo();   //dist-src
            return Ok(employeesDto);
        }

        // employees info - image - email account
        [HttpGet]
        [Route("AllInfo")]
        //[Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetAllInfo()
        {
            var employeesModel = await employeeRepository.GetAllInfoAsync();
            var employeesDto = employeesModel.EmployeesInfo();
            return Ok(employeesDto);
        }

        // employee info - image
        [HttpGet]
        [Route("{id:int}")]
        //[Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var employeeModel = await employeeRepository.GetByIdAsync(id);
            if (employeeModel == null)
            {
                return NotFound();
            }
            var employeeDto = employeeModel.EmployeeProfileInfo();
            return Ok(employeeDto);
        }

        // employee info - image - email account
        [HttpGet]
        //[Authorize(Roles = "Employee")]
        [Route("Info/{id:int}")]
        public async Task<IActionResult> GetByIdInfo([FromRoute] int id)
        {
            var employeeModel = await employeeRepository.GetByIdInfoAsync(id);
            if (employeeModel == null) { return NotFound(); }

            var employeeDto = employeeModel.EmployeeInfo();
            return Ok(employeeDto);
        }

        // update employee info
        [HttpPut]
        [Route("Update/{id:int}")]
        //[Authorize(Roles = "Employee")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] UpdateEmployeeDto updateEmployeeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Update all parameters
            var employeeModel = updateEmployeeDto.EmployeeUpdate();

            //update cv separately
            var old = await employeeRepository.GetByIdAsync(id);
            employeeModel.CVurl = fileService.UpdateFile("CVs", updateEmployeeDto.CV, old.CVurl);

            //update database
            employeeModel = await employeeRepository.UpdateAsync(id, employeeModel);
            if (employeeModel == null)
            {
                return NotFound();
            }

            //convert Domain Model to DTO to pass it to client
            var employeeDto = mapper.Map<EmployeeDto>(employeeModel);
            return Ok(employeeDto);
        }

        // update employee info - image - account
        [HttpPut]
        //[Authorize(Roles = "Employee")]
        [Route("UpdateAll/{id:int}")]
        public async Task<IActionResult> UpdateAllInfo([FromRoute] int id, [FromForm] UpdateEmployeeInfoDto updateEmployeeInfoDto)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        // Map/convert DTO to domain model, Update all parameters
                        var employeeModel = updateEmployeeInfoDto.EmployeeUpdateAll();

                        //Update CVurl parameters
                        var old = await employeeRepository.GetByIdInfoAsync(id);
                        employeeModel.CVurl = fileService.UpdateFile("CVs", updateEmployeeInfoDto.CV, old.CVurl);

                        // Update database
                        var updatedEmployee = await employeeRepository.UpdateAsync(id, employeeModel);
                        if (updatedEmployee == null)
                        {
                            return NotFound("Employee not found"); // Employee not found
                        }

                        // Update profile information
                        var updateProfileDto = new CreateProfileDto
                        {
                            Image = updateEmployeeInfoDto.Image,
                            Type = "Employee"
                        };
                        var profileModel = updateProfileDto.ProfileCreate();
                        profileModel.ImageUrl = fileService.UpdateFile("ProfileImages", updateProfileDto.Image, old.Profile.ImageUrl);
                        profileModel = await profileRepository.UpdateAsync(updatedEmployee.ProfileId, profileModel);

                        // Update account information
                        var accountDomainModel = new Account
                        {
                            UserName = updateEmployeeInfoDto.Email,
                            Email = updateEmployeeInfoDto.Email
                        };
                        var updatedAccount = await accountRepository.GetByIdAsync(updatedEmployee.AccountId);
                        if (updatedAccount == null)
                        {
                            return NotFound("Account not Found");
                        }

                        var updateResult = await accountRepository.UpdateAsync(updatedEmployee.AccountId, accountDomainModel, updateEmployeeInfoDto.Password);

                        if (!updateResult.Succeeded)
                        {
                            transactionScope.Dispose(); // Rollback transaction
                            return BadRequest(updateResult.Errors); // Handle account update failure
                        }

                        transactionScope.Complete(); // Commit transaction
                        return Ok("Employee Updated Successfully");
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

        // update employee info - account
        [HttpPut]
        //[Authorize(Roles = "Employee")]
        [Route("UpdateAll2/{id:int}")]
        public async Task<IActionResult> UpdateAllInfo2([FromRoute] int id, [FromForm] UpdateEmployeeInfo2Dto updateEmployeeInfo2Dto)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        // Map/convert DTO to domain model, Update all parameters
                        var employeeModel = updateEmployeeInfo2Dto.EmployeeUpdateAll2();

                        //Update CVurl parameters
                        var old = await employeeRepository.GetByIdInfoAsync(id);
                        employeeModel.CVurl = fileService.UpdateFile("CVs", updateEmployeeInfo2Dto.CV, old.CVurl);

                        // Update database
                        var updatedEmployee = await employeeRepository.UpdateAsync(id, employeeModel);
                        if (updatedEmployee == null)
                        {
                            return NotFound("Employee not found"); // Employee not found
                        }

                        // Update account information
                        var accountDomainModel = new Account
                        {
                            UserName = updateEmployeeInfo2Dto.Email,
                            Email = updateEmployeeInfo2Dto.Email
                        };
                        var updatedAccount = await accountRepository.GetByIdAsync(updatedEmployee.AccountId);
                        if (updatedAccount == null)
                        {
                            return NotFound("Account not Found");
                        }

                        var updateResult = await accountRepository.UpdateAsync(updatedEmployee.AccountId, accountDomainModel, updateEmployeeInfo2Dto.Password);

                        if (!updateResult.Succeeded)
                        {
                            transactionScope.Dispose(); // Rollback transaction
                            return BadRequest(updateResult.Errors); // Handle account update failure
                        }

                        transactionScope.Complete(); // Commit transaction
                        return Ok("Employee Updated Successfully");
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

        // update employee info - image
        [HttpPut]
        //[Authorize(Roles = "Employee")]
        [Route("UpdateEmployeeProfileAll/{id:int}")]
        public async Task<IActionResult> UpdateAllEmployeeProfileInfo([FromRoute] int id, [FromForm] UpdateEmployeeProfileDto updateEmployeeProfileDto)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        // Map/convert DTO to domain model, Update all parameters
                        var employeeModel = updateEmployeeProfileDto.EmployeeUpdateProfileAll();

                        // Update CVurl parameters
                        var old = await employeeRepository.GetByIdInfoAsync(id);
                        employeeModel.CVurl = fileService.UpdateFile("CVs", updateEmployeeProfileDto.CV, old.CVurl);

                        // Update database
                        var updatedEmployee = await employeeRepository.UpdateAsync(id, employeeModel);
                        if (updatedEmployee == null)
                        {
                            return NotFound(); // Employee not found
                        }

                        // Update profile information
                        var updateProfileDto = new CreateProfileDto
                        {
                            Image = updateEmployeeProfileDto.Image,
                            Type = "Employee"
                        };
                        var profileModel = updateProfileDto.ProfileCreate();
                        profileModel.ImageUrl = fileService.UpdateFile("ProfileImages", updateEmployeeProfileDto.Image, old.Profile.ImageUrl);
                        profileModel = await profileRepository.UpdateAsync(updatedEmployee.ProfileId, profileModel);

                        transactionScope.Complete(); // Commit transaction
                        return Ok("Employee Updated Successfully");
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

        // update employee image
        [HttpPut]
        [Route("UpdateEmployeeProfile/{id:int}")]
        //[Authorize]
        public async Task<IActionResult> UpdateEmployeeProfile([FromRoute] int id, [FromForm] UpdateProfileDto updateProfileDto)
        {
            if (ModelState.IsValid)
            {
                //Update CVurl parameters
                var old = await employeeRepository.GetByIdInfoAsync(id);
                if (old == null)
                {
                    return NotFound();
                }

                var profileModel = new Models.Domain.Profile(); //update type
                profileModel.ImageUrl = fileService.UpdateFile("ProfileImages", updateProfileDto.Image, old.Profile.ImageUrl); //update image,solved
                profileModel = await profileRepository.UpdateAsync(old.ProfileId, profileModel);

                return Ok("Employee Image Updated Successfully");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // update employee account
        [HttpPut]
        //[Authorize(Roles = "Employee")]
        [Route("UpdateEmployeeAccount/{id:int}")]
        public async Task<IActionResult> UpdateEmployeeAccount([FromRoute] int id, [FromBody] UpdateAccountDto updateAccountDto)
        {
            if (ModelState.IsValid)
            {
                var employeeModel = await employeeRepository.GetByIdAsync(id);
                if (employeeModel == null)
                {
                    return NotFound("employee not found"); // employee not found
                }

                // Update account information
                var accountDomainModel = new Account
                {
                    UserName = updateAccountDto.Email,
                    Email = updateAccountDto.Email
                };
                var updatedAccount = await accountRepository.GetByIdAsync(employeeModel.AccountId);
                if (updatedAccount == null)
                {
                    return NotFound("Account not Found");
                }

                var updateResult = await accountRepository.UpdateAsync(employeeModel.AccountId, accountDomainModel, updateAccountDto.Password);

                if (!updateResult.Succeeded)
                {
                    return BadRequest(updateResult.Errors); // Handle account update failure
                }

                return Ok("Employee Updated Successfully");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        //[Authorize(Roles = "Employee")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var employeeModel = await employeeRepository.DeleteAsync(id);
                    fileService.DeleteFile(employeeModel.CVurl);

                    if (employeeModel == null) { return NotFound(); }

                    var profile = await profileRepository.DeleteAsync(employeeModel.ProfileId);
                    fileService.DeleteFile(profile.ImageUrl);

                    var result = await accountRepository.DeleteAsync(employeeModel.AccountId);

                    if (!result)
                    {
                        transactionScope.Dispose(); // Rollback transaction
                        return BadRequest(ModelState);
                    }

                    transactionScope.Complete(); // Commit transaction
                    return Ok("Employee Deleted Successfully");
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
