using AutoMapper;
using EmployMe.CustomActionFilters;
using EmployMe.Models.Domain;
using EmployMe.Models.DTO.JobDto;
using EmployMe.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobRepository _jobRepository;
        private readonly IMapper _mapper;

        public JobController(IJobRepository jobRepository, IMapper mapper)
        {
            _jobRepository = jobRepository;
            _mapper = mapper;
        }

        // Get All jobs
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AvailableJobDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAllJobs(String? query)
        {
            var jobs = await _jobRepository.GetAllJobsAsync(query);

            return Ok(_mapper.Map<List<AvailableJobDto>>(jobs));
        }

        // Get All jobs
        [HttpGet]
        [Route("CompanyJobs{companyId:int}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AvailableJobDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAllJobsCompanyJobs([FromRoute] int companyId, String? query)
        {
            var jobs = await _jobRepository.GetAllJobsCompanyAsync(companyId,query);

            return Ok(_mapper.Map<List<CompanyJobDto>>(jobs));
        }

        // Get one Job by Id
        [HttpGet]
        [Route("{jobId:int}")]
        [ProducesResponseType(200, Type = typeof(AvailableJobDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetJob([FromRoute] int jobId)
        {
            var job = await _jobRepository.GetJobAsync(jobId);

            if (job is null)
                return NotFound();

            return Ok(_mapper.Map<AvailableJobDto>(job));
        }

        // Create Job
        [HttpPost]
        [ValidateModel]
        //[Authorize(Roles = "Company")]
        public async Task<IActionResult> CreateJob([FromBody] AddJobReqDto addJobReqDto)
        {
            var job = _mapper.Map<AvailableJob>(addJobReqDto);
            var result = await _jobRepository.CreateJobAsync(job);

            if (result==null)
            {
                ModelState.AddModelError("", "No more available vacancies");
                return StatusCode(400, ModelState);
            }
            if (result==false)
            {
                ModelState.AddModelError("", "Somthing went wrong while creating the job");
                return StatusCode(500, ModelState);
            }
            return Ok("Created succesfully");
        }

        // Update Job
        [HttpPut]
        [Route("{jobId:int}")]
        [ValidateModel]
        //[Authorize(Roles = "Company")]
        public async Task<IActionResult> UpdateJob([FromRoute] int jobId, [FromBody] UpdateJobReqDto updatedJob)
        {
            var job = _mapper.Map<AvailableJob>(updatedJob);
            if (!await _jobRepository.UpdateJobAsync(jobId, job))
            {
                ModelState.AddModelError("", "Somthing went wrong while updating this job");
                return StatusCode(500, ModelState);
            }

            return Ok("Updated Successfully");
        }

        // Delete Job
        [HttpDelete]
        [Route("{jobId:int}")]
        //[Authorize(Roles = "Company")]
        public async Task<IActionResult> DeleteJob([FromRoute] int jobId)
        {
            if (!await _jobRepository.DeleteJobAsync(jobId))
                return NotFound();

            return Ok("Deleted Successfully");
        }
    }
}
