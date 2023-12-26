using BookStoreView.Models.Dtos.DtoUser;
using System.Security.Claims;

namespace BookStoreBlazorWasm.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(RegisterUserDto registrationModel);
        Task<bool> LoginUserAsync(LoginUserDto loginUserDto);
        Task<IEnumerable<UserDto>> GetUsers();
        public string GetErrorMessage();
        public string GetSuccessMessage();
        Task LogoutUserAsync(List<Claim> claims);
    }
}
