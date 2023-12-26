using AutoMapper;
using BookStoreApi.Models;
using BookStoreApi.Repositories;
using BookStoreApi.Repositories.Interfaces;
using BookStoreView.Models.Dtos;
using BookStoreView.Models.Dtos.DtoUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository userRepository;

        public AddressController(IAddressRepository addressRepository, IMapper mapper, IUserRepository userRepository)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
            this.userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress([FromBody] AddressDto address)
        {
            var result = await _addressRepository.AddAddress(address);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("No old addresses were updated, new address was not added.");
            }
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateOldAddresses(Guid userId)
        {
            var updatedAddressesCount = await _addressRepository.UpdateOldAddresses(userId);
            return Ok($"Updated {updatedAddressesCount} old addresses.");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            var user = await userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = _mapper.Map<UserDto>(user);

            return Ok(userDto);
        }

    }
}
