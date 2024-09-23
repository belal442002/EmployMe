using EmployMe.Models.Domain;
using EmployMe.Models.DTO.AccountDto;
using EmployMe.Models.DTO.CompanyDto;
using EmployMe.Models.DTO.EmployeeDto;
using EmployMe.Models.DTO.ProfileDto;
using System.IO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace EmployMe.Mappings
{
    public static class ManualMapperProfiles
    {
        public static Company CompanyRegister(this RegisterCompanyDto registerCompanyDto)
        {
            return new Company
            {
                ContactEmail = registerCompanyDto.ContactEmail,
                Name = registerCompanyDto.Name,
                Description = registerCompanyDto.Description,
                HR_Email = registerCompanyDto.HR_Email,
            };
        }
        
        public static Company CompanyUpdate(this UpdateCompanyInfoDto updateCompanyInfoDto)
        {
            return new Company
            {
                ContactEmail = updateCompanyInfoDto.ContactEmail,
                Name = updateCompanyInfoDto.Name,
                Description = updateCompanyInfoDto.Description,
                HR_Email = updateCompanyInfoDto.HR_Email,
            };
        }

        public static Company CompanyUpdate2(this UpdateCompanyInfo2Dto updateCompanyInfo2Dto)
        {
            return new Company
            {
                ContactEmail = updateCompanyInfo2Dto.ContactEmail,
                Name = updateCompanyInfo2Dto.Name,
                Description = updateCompanyInfo2Dto.Description,
                HR_Email = updateCompanyInfo2Dto.HR_Email,
            };
        }

        public static Company CompanyProfileUpdate(this UpdateCompanyProfileDto updateCompanyProfileDto)
        {
            return new Company
            {
                ContactEmail = updateCompanyProfileDto.ContactEmail,
                Name = updateCompanyProfileDto.Name,
                Description = updateCompanyProfileDto.Description,
                HR_Email = updateCompanyProfileDto.HR_Email
            };
        }

        public static List<CompanyInfoDto> CompaniesInfo(this List<Company> companies)
        {
            var companyList = new List<CompanyInfoDto>();
            foreach (var company in companies)
            {
                companyList.Add(
                    new CompanyInfoDto
                    {
                        Id = company.Id,
                        ContactEmail = company.ContactEmail,
                        Name = company.Name,
                        Description = company.Description,
                        CommercialRegisterUrl = company.CommercialRegisterUrl,
                        TaxIDUrl = company.TaxIDUrl,
                        HR_Email = company.HR_Email,
                        Available_Job_Interviews = company.Available_Job_Interviews,
                        Available_CV_Recommendations = company.Available_CV_Recommendations,
                        Max_Vacancies = company.Max_Vacancies, 
                        active = company.active,
                        ProfileId = company.ProfileId,
                        ImageUrl = company.Profile.ImageUrl,
                        AccountId = company.AccountId,
                        Email = company.Account.Email,
                        PackageId = company.PackageId,
                    }
                    );
            }
            return companyList;
        }

        public static List<CompanyAdminDto> CompaniesAdminInfo(this List<Company> companies)
        {
            var companyList = new List<CompanyAdminDto>();
            foreach (var company in companies)
            {
                companyList.Add(
                    new CompanyAdminDto
                    {
                        Id = company.Id,
                        Email = company.Account.Email,
                        ContactEmail = company.ContactEmail,
                        Name = company.Name,
                        Description = company.Description,
                        CommercialRegisterUrl = company.CommercialRegisterUrl,
                        TaxIDUrl = company.TaxIDUrl,
                        HR_Email = company.HR_Email
                    }
                    );
            }
            return companyList;
        }

        public static CompanyInfoDto CompanyInfo(this Company company)
        {
            return new CompanyInfoDto
            {
                Id = company.Id,
                ContactEmail = company.ContactEmail,
                Name = company.Name,
                CommercialRegisterUrl = company.CommercialRegisterUrl,
                TaxIDUrl = company.TaxIDUrl,
                HR_Email = company.HR_Email,
                Available_Job_Interviews = company.Available_Job_Interviews,
                Available_CV_Recommendations = company.Available_CV_Recommendations,
                Max_Vacancies = company.Max_Vacancies,
                active = company.active,
                ProfileId = company.ProfileId,
                ImageUrl = company.Profile.ImageUrl,
                AccountId = company.AccountId,
                Email = company.Account.Email,
                PackageId = company.PackageId,
            };
        }

        public static List<CompanyActiveInfoDto> CompaniesActiveInfo(this List<Company> companies)
        {
            var companyList = new List<CompanyActiveInfoDto>();
            foreach (var company in companies)
            {
                companyList.Add(
                    new CompanyActiveInfoDto
                    {
                        Id = company.Id,
                        ContactEmail = company.ContactEmail,
                        Name = company.Name,
                        CommercialRegisterUrl = company.CommercialRegisterUrl,
                        TaxIDUrl = company.TaxIDUrl,
                        HR_Email = company.HR_Email,
                        Available_Job_Interviews = company.Available_Job_Interviews,
                        Available_CV_Recommendations = company.Available_CV_Recommendations,
                        Max_Vacancies= company.Max_Vacancies,
                        ImageUrl = company.Profile.ImageUrl
                    }
                    );
            }
            return companyList;
        }

        public static List<CompanyActiveDto> CompaniesActive(this List<Company> companies)
        {
            var companyList = new List<CompanyActiveDto>();
            foreach (var company in companies)
            {
                companyList.Add(
                    new CompanyActiveDto
                    {
                        ContactEmail = company.ContactEmail,
                        Name = company.Name,
                        HR_Email = company.HR_Email,
                        ImageUrl = company.Profile.ImageUrl
                    }
                    );
            }
            return companyList;
        }

        public static CompanyActiveDto CompanyInformation(this Company company)
        {
            return new CompanyActiveDto
            {
                ContactEmail = company.ContactEmail,
                Name = company.Name,
                HR_Email = company.HR_Email,
                ImageUrl = company.Profile.ImageUrl
            };
        }

        public static Employee EmployeeRegister(this RegisterEmployeeDto registerEmployeeDto)
        {
            return new Employee
            {
                FirstName = registerEmployeeDto.FirstName,
                LastName = registerEmployeeDto.LastName,
                ContactEmail = registerEmployeeDto.ContactEmail,
                ZipCode = registerEmployeeDto.ZipCode,
                City = registerEmployeeDto.City,
                Location = registerEmployeeDto.Location,
                PhoneNumber = registerEmployeeDto.PhoneNumber
            };
        }

        public static List<EmployeeInfoDto> EmployeesInfo(this List<Employee> employees)
        {
            var EmployeeList = new List<EmployeeInfoDto>();
            foreach (var employee in employees)
            {
                EmployeeList.Add(
                    new EmployeeInfoDto
                    {
                        Id = employee.Id,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        ContactEmail = employee.ContactEmail,
                        CVurl = employee.CVurl,
                        ZipCode = employee.ZipCode,
                        City = employee.City,
                        Location = employee.Location,
                        PhoneNumber = employee.PhoneNumber,
                        ProfileId = employee.ProfileId,
                        ImageUrl = employee.Profile.ImageUrl,
                        AccountId = employee.AccountId,
                        Email = employee.Account.Email
                    }
                    );
            }
            return EmployeeList;
        }

        public static List<EmployeeProfileInfoDto> EmployeesProfileInfo(this List<Employee> employees)
        {
            var EmployeeList = new List<EmployeeProfileInfoDto>();
            foreach (var employee in employees)
            {
                EmployeeList.Add(
                    new EmployeeProfileInfoDto
                    {
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        ContactEmail = employee.ContactEmail,
                        CVurl = employee.CVurl,
                        ZipCode = employee.ZipCode,
                        City = employee.City,
                        Location = employee.Location,
                        PhoneNumber = employee.PhoneNumber,
                        ImageUrl = employee.Profile.ImageUrl
                    }
                    );
            }
            return EmployeeList;
        }

        public static EmployeeInfoDto EmployeeInfo(this Employee employee)
        {
            return new EmployeeInfoDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                ContactEmail = employee.ContactEmail,
                CVurl = employee.CVurl,
                ZipCode = employee.ZipCode,
                City = employee.City,
                Location = employee.Location,
                PhoneNumber = employee.PhoneNumber,
                ProfileId = employee.ProfileId,
                ImageUrl = employee.Profile.ImageUrl,
                AccountId = employee.AccountId,
                Email = employee.Account.Email
            };
        }

        public static EmployeeProfileInfoDto EmployeeProfileInfo(this Employee employee)
        {
            return new EmployeeProfileInfoDto
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                ContactEmail = employee.ContactEmail,
                CVurl = employee.CVurl,
                ZipCode = employee.ZipCode,
                City = employee.City,
                Location = employee.Location,
                PhoneNumber = employee.PhoneNumber,
                ImageUrl = employee.Profile.ImageUrl
            };

        }

        public static Employee EmployeeUpdate(this UpdateEmployeeDto updateEmployeeDto)
        {
            return new Employee
            {
                FirstName = updateEmployeeDto.FirstName,
                LastName = updateEmployeeDto.LastName,
                ContactEmail = updateEmployeeDto.ContactEmail,
                ZipCode = updateEmployeeDto.ZipCode,
                City = updateEmployeeDto.City,
                Location = updateEmployeeDto.Location,
                PhoneNumber = updateEmployeeDto.PhoneNumber
            };
        }

        public static Employee EmployeeUpdateAll(this UpdateEmployeeInfoDto updateEmployeeInfoDto)
        {
            return new Employee
            {
                FirstName = updateEmployeeInfoDto.FirstName,
                LastName = updateEmployeeInfoDto.LastName,
                ContactEmail = updateEmployeeInfoDto.ContactEmail,
                ZipCode = updateEmployeeInfoDto.ZipCode,
                City = updateEmployeeInfoDto.City,
                Location = updateEmployeeInfoDto.Location,
                PhoneNumber = updateEmployeeInfoDto.PhoneNumber
            };
        }

        public static Employee EmployeeUpdateAll2(this UpdateEmployeeInfo2Dto updateEmployeeInfo2Dto)
        {
            return new Employee
            {
                FirstName = updateEmployeeInfo2Dto.FirstName,
                LastName = updateEmployeeInfo2Dto.LastName,
                ContactEmail = updateEmployeeInfo2Dto.ContactEmail,
                ZipCode = updateEmployeeInfo2Dto.ZipCode,
                City = updateEmployeeInfo2Dto.City,
                Location = updateEmployeeInfo2Dto.Location,
                PhoneNumber = updateEmployeeInfo2Dto.PhoneNumber
            };
        }

        public static Employee EmployeeUpdateProfileAll(this UpdateEmployeeProfileDto updateEmployeeProfileDto)
        {
            return new Employee
            {
                FirstName = updateEmployeeProfileDto.FirstName,
                LastName = updateEmployeeProfileDto.LastName,
                ContactEmail = updateEmployeeProfileDto.ContactEmail,
                ZipCode = updateEmployeeProfileDto.ZipCode,
                City = updateEmployeeProfileDto.City,
                Location = updateEmployeeProfileDto.Location,
                PhoneNumber = updateEmployeeProfileDto.PhoneNumber
            };
        }
       
        public static Profile ProfileCreate(this CreateProfileDto createProfileDto)
        {
            return new Profile
            {
                Type = createProfileDto.Type,
            };
        }

        public static Account AccountUpdate(this UpdateAccDto updateAccDto)
        {
            return new Account
            {
                UserName = updateAccDto.Email,
                Email = updateAccDto.Email

            };
        }

        public static Account AddAccount(this AddAccountDto addAccountDto)
        {
            return new Account
            {
                UserName = addAccountDto.Email,
                Email = addAccountDto.Email,
                Type = addAccountDto.Type
            };
        }
    }
}
