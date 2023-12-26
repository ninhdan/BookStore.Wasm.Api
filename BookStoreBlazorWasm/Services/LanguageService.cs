using Blazored.Toast.Services;
using BookStoreBlazorWasm.Services.Interfaces;
using BookStoreView.Models.Dtos;
using System.Net.Http.Json;
using System.Net;
using Newtonsoft.Json;
using System.Text;

namespace BookStoreBlazorWasm.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly HttpClient _httpClient;
        //private readonly ToastService toastService = new ToastService();
        private string errorMessage;
        private string successMessage;
        public LanguageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> CreateLanguage(LanguageDto languageDto)
        {
            try
            {
                // Gửi yêu cầu HTTP POST để tạo mới danh mục
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/language", languageDto);
                if (response.IsSuccessStatusCode)
                {
                    successMessage = $"Add {languageDto.LanguageName} Success !";
                    Console.WriteLine($"Success:{response.IsSuccessStatusCode}, Content: {await response.Content.ReadAsStringAsync()}");
                    return true;
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.Conflict)
                    {

                        errorMessage = $"{languageDto.LanguageName} already exists";
                        Console.WriteLine($"Error:{response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()} ");
                        return false;
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        Console.WriteLine($"Error:{response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()} ");
                        return false;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        Console.WriteLine($"Error:{response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()} ");
                        return false;
                    }
                    else
                    {
                        Console.WriteLine($"Error:{response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()} ");
                        errorMessage = $"Add {languageDto.LanguageName} Category Fail | Error: {response.StatusCode}";
                        return false;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                // Xử lý lỗi HTTP (ví dụ: mất kết nối)

                Console.WriteLine($"HTTP Request Error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Xử lý các ngoại lệ khác
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteLanguage(Guid languageId)
        {
            try
            {
                // Gửi yêu cầu HTTP DELETE để xóa danh mục
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/language/{languageId}");
                if (response.IsSuccessStatusCode)
                {
                    successMessage = $"Delete language Success !";
                    Console.WriteLine($"Success:{response.IsSuccessStatusCode}, Content: {await response.Content.ReadAsStringAsync()}");
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (HttpRequestException ex)
            {
                // Xử lý lỗi HTTP (ví dụ: mất kết nối)
                Console.WriteLine($"HTTP Request Error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Xử lý các ngoại lệ khác
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public string GetErrorMessage()
        {
            return errorMessage;
        }

        public async Task<IEnumerable<LanguageDto>> GetLanguages()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/language");

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<IEnumerable<LanguageDto>>();
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
                Console.WriteLine("Lỗi ngay đây");
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        public async Task<LanguageDto> GetLanguage(Guid languageId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/language/{languageId}");

                // Đảm bảo rằng mã trạng thái là thành công (2xx)
                response.EnsureSuccessStatusCode();

                // Đọc và trả về dữ liệu nếu không có lỗi
                return await response.Content.ReadFromJsonAsync<LanguageDto>();
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
                        // Xử lý trường hợp không tìm thấy
                        errorMessage = "NotFound Author";
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

        public string GetSuccessMessage()
        {
            return successMessage;
        }

        public async Task<bool> UpdateLanguage(LanguageDto languageDto)
        {
            if (languageDto == null)
            {
                throw new ArgumentNullException(nameof(languageDto));
            }

            try
            {
                // Chuyển đổi đối tượng DTO thành chuỗi JSON
                string languageJson = JsonConvert.SerializeObject(languageDto);
                StringContent content = new StringContent(languageJson, Encoding.UTF8, "application/json");

                // Gửi yêu cầu HTTP PUT để cập nhật danh mục
                HttpResponseMessage response = await _httpClient.PutAsync($"api/language/{languageDto.LanguageId}", content);

                if (response.IsSuccessStatusCode)
                {
                    successMessage = $"Update {languageDto.LanguageName} Success !";
                    Console.WriteLine($"Success:{response.IsSuccessStatusCode}, Content: {await response.Content.ReadAsStringAsync()}");
                    return true;

                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.Conflict)
                    {
                        errorMessage = $"{languageDto.LanguageName} already exists";
                        Console.WriteLine($"Error:{response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()} ");
                        return false;
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        Console.WriteLine($"Error:{response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()} ");
                        return false;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {

                        Console.WriteLine($"Error:{response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()} ");
                        return false;
                    }
                    else
                    {
                        errorMessage = $"Update {languageDto.LanguageName} Author Fail | Error: {response.StatusCode}";
                        return false;
                    }
                }

            }
            catch (HttpRequestException ex)
            {
                // Xử lý lỗi HTTP (ví dụ: mất kết nối)
                Console.WriteLine($"HTTP Request Error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Xử lý các ngoại lệ khác
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }


      



    }
}

       


