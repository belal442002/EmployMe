using AutoMapper;
using EmployMe.CustomActionFilters;
using EmployMe.Models.Domain;
using EmployMe.Models.DTO.PackageDto;
using EmployMe.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IMapper _mapper;

        public PackageController(IPackageRepository packageRepository, IMapper mapper)
        {
            _packageRepository = packageRepository;
            _mapper = mapper;
        }

        // Get All Packages
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PackageDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAllPackages([FromQuery] bool? activePackage = true)
        {
            var packages = await _packageRepository.GetAllPackagesAsync(activePackage ?? true);

            return Ok(_mapper.Map<List<PackageDto>>(packages));
        }

        // Gat Package by Id
        [HttpGet]
        [Route("{packageId:int}")]
        [ProducesResponseType(200, Type = typeof(PackageDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPackage([FromRoute] int packageId)
        {
            var package = await _packageRepository.GetPackageAsync(packageId);

            if (package == null)
                return NotFound();

            return Ok(_mapper.Map<PackageDto>(package));
        }

        // Create Package
        [HttpPost]
        [ValidateModel]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePackage([FromBody] AddPackageReqDto addPackageReqDto)
        {
            if(await _packageRepository.ExistAsync(addPackageReqDto.Name))
            {
                ModelState.AddModelError("", "Package name already exists");
                return StatusCode(422, ModelState);
            }

            var package = _mapper.Map<Package>(addPackageReqDto);

            if(!await _packageRepository.CreatePackageAsync(package))
            {
                ModelState.AddModelError("", "Somthing went wrong while creating the package");
                return StatusCode(500, ModelState);
            }

            return Ok("Package Created Successfully");
        }

        // Update Package
        [HttpPut]
        [Route("{packageId:int}")]
        [ValidateModel]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePackage([FromRoute] int packageId, [FromBody] UpdatePackageReqDto updatedPackage)
        {
            if(await _packageRepository.ExistAsync(updatedPackage.Name))
            {
                ModelState.AddModelError("", "Package name already exist");
                return StatusCode(422, ModelState);
            }

            var package = _mapper.Map<Package>(updatedPackage);
            if(!await _packageRepository.UpdatePackageAsync(packageId, package))
            {
                ModelState.AddModelError("", "Somthing went wrong while saving package");
                return StatusCode(500, ModelState);
            }

            return Ok("Updated Successfully");
        }

        // Delate Package
        [HttpDelete]
        [Route("{packageId:int}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DelatePackage([FromRoute] int packageId)
        {
            if (!await _packageRepository.DeletePackageAsync(packageId))
                return NotFound();

            return Ok("Deleted Successfully");
        }
    }

}
