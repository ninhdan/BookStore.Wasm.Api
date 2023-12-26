using BookStoreApi.Models;
using BookStoreView.Models.Dtos.DtoUser;

namespace BookStoreApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<(bool IsUserRegistered, string Message)> RegisterNewUser(RegisterUserDto userRegistration);
        Task<(bool IsLoginSucess, JWTTokenDto TokenResponse)> LoginAsync(LoginUserDto loginpayload);
        Task<(string ErrorsMessage, JWTTokenDto JWTTokenResponse)> RenewTokenAsync(RenewTokenDto renewTokenRequest);
        Task UserLogoutAsync(LogoutDto logoutRequest);
        ICollection<User> GetUsers();
        Task<User> GetUserByIdAsync(Guid userId);
    }
}
