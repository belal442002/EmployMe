using AutoMapper;
using EmployMe.FileUploadService;
using EmployMe.Mappings;
using EmployMe.Models.DTO.ProfileDto;
using EmployMe.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EmployMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IProfileRepository profileRepository;
        private readonly IMapper mapper;
        private readonly FileService fileService;

        public ProfilesController(IProfileRepository profileRepository, IMapper mapper, FileService fileService)
        {
            this.profileRepository = profileRepository;
            this.mapper = mapper;
            this.fileService = fileService;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var profilesModel = await profileRepository.GetAllAsync();
            var profilesDto = mapper.Map<List<ProfileDto>>(profilesModel);   //dist-src
            return Ok(profilesDto);
        }

        [HttpGet]
        [Route("{id:int}")]
        //[Authorize]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var profileModel = await profileRepository.GetByIdAsync(id);
            if (profileModel == null)
            {
                return NotFound();
            }
            var profileDto = mapper.Map<ProfileDto>(profileModel);
            return Ok(profileDto);
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> Create([FromForm] CreateProfileDto createProfileDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var profileModel = createProfileDto.ProfileCreate();
            profileModel.ImageUrl = fileService.UploadFile("ProfileImages", createProfileDto.Image); //image handled to string;
            profileModel = await profileRepository.CreateAsync(profileModel);
            var profileDto = mapper.Map<ProfileDto>(profileModel);
            return Ok(profileDto);
        }

        [HttpPut]
        [Route("Update/{id:int}")]
        //[Authorize]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] CreateProfileDto updateProfileDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Update all parameters
            var profileModel = updateProfileDto.ProfileCreate();
            //update image separately
            var old = await profileRepository.GetByIdAsync(id);
            profileModel.ImageUrl = fileService.UpdateFile("ProfileImages", updateProfileDto.Image, old.ImageUrl);
            //update database
            profileModel = await profileRepository.UpdateAsync(id, profileModel);
            if (profileModel == null)
            {
                return NotFound();
            }
            var profileDto = mapper.Map<ProfileDto>(profileModel);
            return Ok(profileDto);
        }

        [HttpDelete]
        [Route("{id:int}")]
        //[Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var profileModel = await profileRepository.DeleteAsync(id);
            //remove from wwwroot
            fileService.DeleteFile(profileModel.ImageUrl);
            if (profileModel == null)
            {
                return NotFound();
            }
            var profileDto = mapper.Map<ProfileDto>(profileModel);
            return Ok(profileDto);
        }
    }
}
