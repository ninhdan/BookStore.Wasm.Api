using BookStoreApi.Models;
using BookStoreApi.Repositories.Interfaces;
using BookStoreApi.Shared.Settings;
using BookStoreView.Models.Dtos.DtoUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace BookStoreApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbWater7Context context;
        private readonly TokenSettings _tokenSettings;

        public UserRepository(DbWater7Context context, IOptions<TokenSettings> tokenSettings) {
            this.context = context;
            _tokenSettings = tokenSettings.Value;
        }

        public ICollection<User> GetUsers()
        {
            return context.Users.ToList();
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await context.Users
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        private User FromUserRegistrationModelToUserModel(RegisterUserDto userRegistration)
        {
            return new User
            {
                Phone = userRegistration.Phone,
                Firstname = userRegistration.FirstName,
                Lastname = userRegistration.LastName,
                Password = userRegistration.Password,
            };
        }

        private string HashPassword(string plainPassword)
        {
            byte[] salt = new byte[16];
            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var rfcPassword = new Rfc2898DeriveBytes(plainPassword, salt, 1000, HashAlgorithmName.SHA1);

            byte[] rfcPasswordHash = rfcPassword.GetBytes(20);
            byte[] passwordHash = new byte[36];
            Array.Copy(salt, 0, passwordHash, 0, 16);
            Array.Copy(rfcPasswordHash,0,passwordHash, 16, 20);

            return Convert.ToBase64String(passwordHash);

        }

        public async Task<(bool IsUserRegistered, string Message)> RegisterNewUser(RegisterUserDto userRegistration)
        {
            var isUserExist = context.Users.Any(_ => _.Phone == userRegistration.Phone);
            if (isUserExist)
            {
                return (false, "Phone Already Registred");
            }

            var newUser = FromUserRegistrationModelToUserModel(userRegistration);
            newUser.Password = HashPassword(newUser.Password);

            context.Users.Add(newUser);

                await context.SaveChangesAsync();
                return (true, "Success");
           
        }

        private bool PasswordVerification(string plainPassword, string dbPassword)
        {
            byte[] dbPasswordHash = Convert.FromBase64String(dbPassword);

            byte[] salt = new byte[16];
            Array.Copy(dbPasswordHash, 0, salt, 0, 16);

            var rfcPassowrd = new Rfc2898DeriveBytes(plainPassword, salt, 1000, HashAlgorithmName.SHA1);
            byte[] rfcPasswordHash = rfcPassowrd.GetBytes(20);

            for (int i = 0; i < rfcPasswordHash.Length; i++)
            {
                if (dbPasswordHash[i + 16] != rfcPasswordHash[i])
                {
                    return false;
                }
            }
            return true;
        }

        private string GenerateJwtAccessToken(User user)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.SecretKey));

            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>();
            claims.Add(new Claim("Sub", user.UserId.ToString()));
            claims.Add(new Claim("FirstName", user?.Firstname ?? string.Empty));
            claims.Add(new Claim("LastName", user?.Lastname ?? string.Empty));
            claims.Add(new Claim("Phone", user?.Phone ?? string.Empty));
			claims.Add(new Claim(ClaimTypes.Role, user.Role));

			var securityToken = new JwtSecurityToken(
                issuer: _tokenSettings.Issuer,
                audience: _tokenSettings.Audience,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials,
                claims: claims);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }


        public async Task<(bool IsLoginSucess, JWTTokenDto TokenResponse)> LoginAsync(LoginUserDto loginpayload)
        {


			if (string.IsNullOrEmpty(loginpayload.Phone) || string.IsNullOrEmpty(loginpayload.Password))
            {
                return (false, null);
            }

            var user = await context.Users.Where(_ => _.Phone == loginpayload.Phone)
                .FirstOrDefaultAsync();

            if (user == null) { return (false, null); }

            bool validUserPassowrd = PasswordVerification(loginpayload.Password, user.Password);
            if (!validUserPassowrd) { return (false, null); }

            string jwtAccessToken = GenerateJwtAccessToken(user);
            string refreshToken = await GenerateRefreshToken(user.UserId);
            var result = new JWTTokenDto
            {
                AccessToken = jwtAccessToken,
                RefreshToken = refreshToken,

            };
			return (true, result);
        }

        private async Task<string> GenerateRefreshToken(Guid userId)
        {
            byte[] bytetoken = new byte[32];
            using (var randomGenerator = RandomNumberGenerator.Create())
            {
                randomGenerator.GetBytes(bytetoken);
            }
            var token = Convert.ToBase64String(bytetoken);
            var newRefreshToken = new UserRefreshToken
            {
                UserId = userId,
                Token = token,
                ExpirationDate = DateTime.UtcNow.AddDays(3),
            };
            context.UserRefreshTokens.Add(newRefreshToken);
            await context.SaveChangesAsync();
            return token;
        }

        public async Task<(string ErrorsMessage, JWTTokenDto JWTTokenResponse)> RenewTokenAsync(RenewTokenDto renewTokenRequest)
        {
            var existingRefreshToken = await context.UserRefreshTokens
                .Where(_ => _.UserId == renewTokenRequest.UserId
                && _.Token == renewTokenRequest.RefreshToken &&
                _.ExpirationDate > DateTime.Now).FirstOrDefaultAsync();

            if (existingRefreshToken == null)
            {
                return ("Invalid Refresh Token", null);
            }
            context.Remove(existingRefreshToken);
            await context.SaveChangesAsync();

            var user = await context.Users.Where(_ => _.UserId == renewTokenRequest.UserId).FirstOrDefaultAsync();

            string jwtAccessToken = GenerateJwtAccessToken(user);
            string refreshToken = await GenerateRefreshToken(user.UserId);
            var result = new JWTTokenDto
            {
                AccessToken = jwtAccessToken,
                RefreshToken = refreshToken,
            };
            return ("", result);
        }


        public async Task UserLogoutAsync(LogoutDto logoutRequest)
        {
            var tokenToDelete = await context.UserRefreshTokens
                .Where(_ => _.UserId == logoutRequest.UserId && _.Token == logoutRequest.RefreshToken).FirstOrDefaultAsync();

            if (tokenToDelete != null)
            {
                context.Remove(tokenToDelete);
                await context.SaveChangesAsync();
            }
        }

    }
}

