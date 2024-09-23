using AutoMapper;
using EmployMe.Models.Domain;
using EmployMe.Models.DTO;
using EmployMe.Models.DTO.ApplicationDto;
using EmployMe.Models.DTO.CompanyDto;
using EmployMe.Models.DTO.EmployeeDto;
using EmployMe.Models.DTO.JobDto;
using EmployMe.Models.DTO.PackageDto;
using EmployMe.Models.DTO.ProfileDto;

namespace EmployMe.Mapper
{
    public class AutoMapperProfiles : AutoMapper.Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Company, CompanyDto>().ReverseMap(); //src-dest
            CreateMap<Company, RegisterCompanyDto>().ReverseMap();
            CreateMap<Company, UpdateCompanyDto>().ReverseMap();
            CreateMap<Company, UpdateCompanyCvRDto>().ReverseMap();
            CreateMap<Company, UpdateCompanyJobInterviewsDto>().ReverseMap();
            CreateMap<Company, UpdateCompanyCvRnadInterviewsDto>().ReverseMap();
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<Employee, EmployeeInformationDto>().ReverseMap();
            CreateMap<Models.Domain.Profile, ProfileDto>().ReverseMap();

            CreateMap<Application, ApplicationDto>().ReverseMap();
            CreateMap<Application, AddApplicationReqDto>().ReverseMap();
            CreateMap<Application, CompanyApplicationDto>().ReverseMap();
            CreateMap<AvailableJob, AvailableJobDto>().ReverseMap();
            CreateMap<AvailableJob, CompanyJobDto>().ReverseMap();
            CreateMap<AvailableJob, AddJobReqDto>().ReverseMap();
            CreateMap<AvailableJob, UpdateJobReqDto>().ReverseMap();
            CreateMap<Package, PackageDto>().ReverseMap();
            CreateMap<Package, AddPackageReqDto>().ReverseMap();
            CreateMap<Package, UpdatePackageReqDto>().ReverseMap();
        }
    }
}
