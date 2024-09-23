using AutoMapper;
using EmployMe.CustomActionFilters;
using EmployMe.Models.Domain;
using EmployMe.Models.DTO.ApplicationDto;
using EmployMe.Models.DTO.EmployeeDto;
using EmployMe.Models.DTO.JobDto;
using EmployMe.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EmployMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IMapper _mapper;

        public ApplicationController(IApplicationRepository applicationRepository, IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _mapper = mapper;
        }

        // Get All Applications
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ApplicationDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAllApplications([FromQuery] bool? getAccepted)
        {
            var applications = await _applicationRepository.GetAllApplicationsAsync(getAccepted);
            if (applications == null)
                return NotFound();

            return Ok(_mapper.Map<List<ApplicationDto>>(applications));
        }

        // Get Application
        [HttpGet]
        [Route("{empId:int}/{jobId:int}")]
        [ProducesResponseType(200, Type = typeof(ApplicationDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetApplication([FromRoute] int empId, [FromRoute] int jobId)
        {
            var application = await _applicationRepository.GetApplicationAsync(empId, jobId);
            if (application is null)
                return NotFound();

            return Ok(_mapper.Map<ApplicationDto>(application));
        }

        // Create Application
        [HttpPost]
        [ValidateModel]
        //[Authorize(Roles = "Employee")]
        public async Task<IActionResult> CreateApplication([FromBody] AddApplicationReqDto application)
        {
            var applicationDM = _mapper.Map<Application>(application);
            if (!await _applicationRepository.CreateApplicationAsync(applicationDM))
            {
                ModelState.AddModelError("", "Somthing went wrong while creating the application");
                return StatusCode(500, ModelState);
            }

            return Ok("Application Created Successfully");
        }

        // Delate Application
        [HttpDelete]
        [Route("{employeeId:int}/{jobId:int}")]
        //[Authorize(Roles = "Employee")]
        public async Task<IActionResult> DeleteApplication([FromRoute] int employeeId, [FromRoute] int jobId)
        {
            if(!await _applicationRepository.DeleteApplicationAsync(employeeId, jobId))
            {
                ModelState.AddModelError("", "Somthing went wrong while deleting the application");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted Successfully");
        }

        // Accept Applicant
        [HttpPut]
        [Route("AcceptApplicant/{emplyeeId:int}/{jobId:int}")]
        public async Task<IActionResult> AcceptApplicant([FromRoute] int emplyeeId, [FromRoute] int jobId)
        {
            if(!await _applicationRepository.AcceptApplicantAsync(emplyeeId, jobId))
            {
                ModelState.AddModelError("", "Somthing went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Applicant Accepted Successfully");
        }

        // Get Applicants By Job Id
        [HttpGet]
        [Route("GetApplicantsByJob/{jobId:int}")]
        public async Task<IActionResult> GetApplicantsByJobAsync([FromRoute] int jobId)
        {
            var applicants = await _applicationRepository.GetApplicantsByJobAsync(jobId);

            if (applicants is null)
                return NotFound();

            return Ok(_mapper.Map<List<EmployeeDto>>(applicants));
        }

        // Get Company Applications
        [HttpGet]
        [Route("GetCompanyApplications/{companyId}")]
        public async Task<IActionResult> GetCompanyApplications([FromRoute] int companyId)
        {
            var applications = await _applicationRepository.GetCompanyApplicationsAsync(companyId);

            if (applications is null)
                return NotFound();

            return Ok(_mapper.Map<List<CompanyApplicationDto>>(applications));
        }

        // Get Jobs By Employee Id
        [HttpGet]
        [Route("GetJobsByEmployee/{employeeId:int}")]
        public async Task<IActionResult> GetJobsByEmployee([FromRoute] int employeeId)
        {
            var jobs = await _applicationRepository.GetJobsByEmployeeAsync(employeeId);

            if (jobs is null)
                return NotFound();

            return Ok(_mapper.Map<List<AvailableJobDto>>(jobs));
        }

    }

}
