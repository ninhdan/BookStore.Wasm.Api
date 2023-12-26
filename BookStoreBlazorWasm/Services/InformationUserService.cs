using BookStoreBlazorWasm.Services.Interfaces;
using BookStoreView.Models.Dtos;
using BookStoreView.Models.Dtos.DtoUser;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace BookStoreBlazorWasm.Services
{
    public class InformationUserService : IInformationUserService
    {
        private readonly HttpClient _httpClient;

        public InformationUserService(HttpClient httpClient) {
            this._httpClient = httpClient;
        }

        public async Task<bool> UpdateUser(UpdateUserDto userUpdateDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/InformationUser/{userUpdateDto.UserId}", userUpdateDto);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false; // or throw an exception
            }
        }

        public async Task<UpdateUserDto> GetUserById(Guid userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/InformationUser/{userId}");

                // Đảm bảo rằng mã trạng thái là thành công (2xx)
                response.EnsureSuccessStatusCode();

                // Đọc và trả về dữ liệu nếu không có lỗi
                return await response.Content.ReadFromJsonAsync<UpdateUserDto>();
            }
            catch (HttpRequestException ex)
            {
                // Xử lý lỗi HTTP
                Console.WriteLine($"HTTP Request Error: {ex.Message}");

                // Kiểm tra mã trạng thái
                if (ex.StatusCode.HasValue)
                {
                    if (ex.StatusCode == HttpStatusCode.NotFound)
                    {
                       
                    }
                }

                // Nếu không phải là lỗi 404, có thể xử lý khác tùy thuộc vào yêu cầu của bạn
                throw;
            }
            catch (Exception ex)
            {
                // Xử lý các ngoại lệ khác, có thể là ghi log hoặc xử lý khác
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }

        }

    }


}

