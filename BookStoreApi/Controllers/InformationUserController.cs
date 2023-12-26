using AutoMapper;
using BookStoreApi.Repositories;
using BookStoreApi.Repositories.Interfaces;
using BookStoreView.Models.Dtos;
using BookStoreView.Models.Dtos.DtoUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformationUserController : ControllerBase
    {
        private readonly IInformationUserRepository informationUserRepository;
        private readonly IMapper mapper;

        public InformationUserController(IInformationUserRepository informationUserRepository, IMapper mapper) {

            this.informationUserRepository = informationUserRepository;
            this.mapper = mapper;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateUserDto userUpdateDto)
        {
            if (id != userUpdateDto.UserId)
            {
                return BadRequest();
            }
            try
            {
                var userDto = await informationUserRepository.UpdateUser(userUpdateDto);
                return Ok(userDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{userId:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            if (!informationUserRepository.UserExists(userId))
            {
                return NotFound();
            }

            var user = await informationUserRepository.GetUserById(userId);
            var UpdateUserDto = mapper.Map<UpdateUserDto>(user);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(UpdateUserDto);
        }

    }
}
