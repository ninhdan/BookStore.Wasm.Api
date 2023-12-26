using BookStoreBlazorWasm.Services.Interfaces;
using BookStoreView.Models.Dtos.DtoUser;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BookStoreView.Models.Dtos;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using System.Net.Http;
using BookStoreBlazorWasm.Shared.Providers;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace BookStoreBlazorWasm.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly NavigationManager _navigationManager;
        private string errorMessage;
        private string successMessage;

        public UserService(HttpClient httpClient, ILocalStorageService localStorageService, AuthenticationStateProvider authStateProvider, NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _authStateProvider = authStateProvider;
            _navigationManager = navigationManager;
        }

        public string GetErrorMessage()
        {
            return errorMessage;
        }

        public string GetSuccessMessage()
        {
            return successMessage;
        }

        public async Task<bool> RegisterUserAsync(RegisterUserDto registrationDto)
        {
            var jsonPayload = JsonSerializer.Serialize(registrationDto);
            var requestContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/user/register", requestContent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                // Đăng ký thành công
                successMessage = $"Đăng ký {registrationDto.FirstName} {registrationDto.LastName} Success !";
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // Đã xảy ra lỗi BadRequest
                var errors = await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>();
                if (errors.ContainsKey("Phone"))
                {
                    // Số điện thoại đã tồn tại
                    errorMessage =  $"Số {registrationDto.Phone} đã được sử dụng. Vui lòng chọn số điện thoại khác.";

                }
                else
                {
                    // Xử lý các lỗi khác (nếu có)
                    // ...
                    errorMessage = "Đăng ký không thành công. Vui lòng thử lại.";
                }
                return false;
            }
            else
            {
                // Xử lý các trạng thái khác (nếu có)
                // ...
                errorMessage = "Đăng ký không thành công. Vui lòng thử lại.";
                return false;
            }
        }

        public async Task<bool> LoginUserAsync(LoginUserDto loginUserDto)
        {
            var jsonPayload = JsonSerializer.Serialize(loginUserDto);
            var requestContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/user/login", requestContent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var tokenResponse = await response.Content.ReadFromJsonAsync<JWTTokenDto>();
                Console.WriteLine($"Success:{response.IsSuccessStatusCode}, Content: {tokenResponse}");
                successMessage = $"Đăng nhập thành công ";

                await _localStorageService.SetItemAsync<string>("jwt-access-token", tokenResponse.AccessToken);
                await _localStorageService.SetItemAsync<string>("refresh-token", tokenResponse.RefreshToken);
                (_authStateProvider as CustomAuthProvider).NotifyAuthState();

                var isAdmin = IsAdmin(tokenResponse.AccessToken);

                if (isAdmin)
                {
					_navigationManager.NavigateTo("/admin");
				}
                else
                {
					_navigationManager.NavigateTo("/");
				}
                return true;
            }
            else
            {
                Console.WriteLine($"Success:{response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()}");
                errorMessage = $"Số điện thoại hoặc mật khẩu không đúng";
                return false;
            }

        }

        private bool IsAdmin(string jwtToken)
        {
			try
			{
				var handler = new JwtSecurityTokenHandler();
				var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;

				if (jsonToken != null)
				{
					foreach (var claim in jsonToken.Claims)
					{
						Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
					}
				}

				return jsonToken?.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" && c.Value == "Admin") != null;
			}
			catch (Exception e)
			{
				// Xử lý lỗi ở đây
				Console.WriteLine($"An error occurred: {e.Message}");
				return false;
			}
		}

        public async Task LogoutUserAsync(List<Claim> claims)
        {
            var logout = new LogoutDto();

            // Chuyển đổi từ chuỗi "Sub" thành Guid
            logout.UserId = Guid.Parse(claims.Where(_ => _.Type == "Sub").Select(_ => _.Value).FirstOrDefault() ?? "0");

            logout.RefreshToken = await _localStorageService.GetItemAsync<string>("refresh-token");

            var jsonPayload = JsonSerializer.Serialize(logout);
            var requestContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
         
            var response = await _httpClient.PostAsync("/api/user/logout", requestContent);

            await _localStorageService.RemoveItemAsync("refresh-token");
            await _localStorageService.RemoveItemAsync("jwt-access-token");

            (_authStateProvider as CustomAuthProvider).NotifyAuthState();
            _navigationManager.NavigateTo("/login");
        }

        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/user");

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<IEnumerable<UserDto>>();
            }
            catch (HttpRequestException ex)
            {
                // Xử lý lỗi HTTP (ví dụ: mất kết nối)
                Console.WriteLine($"HTTP Request Error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Xử lý các ngoại lệ khác
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }





    }


}

