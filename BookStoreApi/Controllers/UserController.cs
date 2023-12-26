using AutoMapper;
using BookStoreApi.Models;
using BookStoreApi.Repositories;
using BookStoreApi.Repositories.Interfaces;
using BookStoreView.Models.Dtos;
using BookStoreView.Models.Dtos.DtoUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UserController(IUserRepository userRepository, IMapper mapper) {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> UserRegistration(RegisterUserDto registerUserDto)
        {
            var result = await userRepository.RegisterNewUser(registerUserDto);
            if (result.IsUserRegistered)
            {
                return Ok(result.Message);
            }
            ModelState.AddModelError("Phone", result.Message);
            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginUserDto loginPayload)
        {
            var result = await userRepository.LoginAsync(loginPayload);
            if (result.IsLoginSucess)
            {
                return Ok(result.TokenResponse);
            }

            ModelState.AddModelError("LoginError", "Invalid phone or password");
            return BadRequest(ModelState);
        }

        // GET: api/Users
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUsers()
        {
            var users = userRepository.GetUsers();
            var userDto = new List<UserDto>();
            foreach (var user in users)
            {
                userDto.Add(mapper.Map<UserDto>(user));
            }
            return Ok(userDto);
        }

        [HttpPost("renew-tokens")]
        public async Task<IActionResult> RenewTokensAsync(RenewTokenDto renewTokenRequest)
        {
            var result = await userRepository.RenewTokenAsync(renewTokenRequest);
            if (!string.IsNullOrEmpty(result.ErrorsMessage))
            {
                return BadRequest(result.ErrorsMessage);
            }
            return Ok(result.JWTTokenResponse);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutDto logoutRequest)
        {
            await userRepository.UserLogoutAsync(logoutRequest);
            return Ok();
        }

    }
}
