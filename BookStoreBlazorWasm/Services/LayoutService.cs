
using BookStoreBlazorWasm.Services.Interfaces;
using BookStoreView.Models.Dtos;
using System.Net.Http.Json;
using System.Net;
using Newtonsoft.Json;
using System.Text;

namespace BookStoreBlazorWasm.Services
{
    public class LayoutService : ILayoutService
    {
        private readonly HttpClient _httpClient;
        private string errorMessage;
        private string successMessage;
        public LayoutService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> CreateLayout(LayoutDto layoutDto)
        {
            try
            {
                // Gửi yêu cầu HTTP POST để tạo mới danh mục
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/layout", layoutDto);
                if (response.IsSuccessStatusCode)
                {
                    successMessage = $"Add {layoutDto.LayoutName} Success !";
                    Console.WriteLine($"Success:{response.IsSuccessStatusCode}, Content: {await response.Content.ReadAsStringAsync()}");
                    return true;
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.Conflict)
                    {

                        errorMessage = $"{layoutDto.LayoutName} already exists";
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
                        errorMessage = $"Add {layoutDto.LayoutName} Category Fail | Error: {response.StatusCode}";
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

        public string GetErrorMessage()
        {
            return errorMessage;
        }
        public string GetSuccessMessage()
        {
            return successMessage;
        }

        public async Task<IEnumerable<LayoutDto>> GetLayouts()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/layout");

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<IEnumerable<LayoutDto>>();
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

     

        public async Task<bool> UpdateLayout(LayoutDto layoutDto)
        {
            if (layoutDto == null)
            {
                throw new ArgumentNullException(nameof(layoutDto));
            }

            try
            {
                // Chuyển đổi đối tượng DTO thành chuỗi JSON
                string categoryJson = JsonConvert.SerializeObject(layoutDto);
                StringContent content = new StringContent(categoryJson, Encoding.UTF8, "application/json");

                // Gửi yêu cầu HTTP PUT để cập nhật danh mục
                HttpResponseMessage response = await _httpClient.PutAsync($"api/layout/{layoutDto.LayoutId}", content);

                if (response.IsSuccessStatusCode)
                {
                    successMessage = $"Update {layoutDto.LayoutName} Success !";
                    Console.WriteLine($"Success:{response.IsSuccessStatusCode}, Content: {await response.Content.ReadAsStringAsync()}");
                    return true;

                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.Conflict)
                    {
                        errorMessage = $"{layoutDto.LayoutName} already exists";
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
                        errorMessage = $"Update {layoutDto.LayoutName} Author Fail | Error: {response.StatusCode}";
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

        public async Task<LayoutDto> GetLayout(Guid layoutId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/layout/{layoutId}");

                // Đảm bảo rằng mã trạng thái là thành công (2xx)
                response.EnsureSuccessStatusCode();

                // Đọc và trả về dữ liệu nếu không có lỗi
                return await response.Content.ReadFromJsonAsync<LayoutDto>();
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
                        errorMessage = "NotFound Layout";
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

        public async Task<bool> DeleteLayout(Guid layoutId)
        {
            try
            {
                // Gửi yêu cầu HTTP DELETE để xóa danh mục
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/layout/{layoutId}");
                if (response.IsSuccessStatusCode)
                {
                    successMessage = $"Delete layout Success !";
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


    }
    
}
