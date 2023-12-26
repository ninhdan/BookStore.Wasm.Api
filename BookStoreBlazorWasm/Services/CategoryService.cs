using Blazored.Toast.Services;
using BookStoreBlazorWasm.Services.Interfaces;
using BookStoreView.Models.Dtos;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace BookStoreBlazorWasm.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private string errorMessage;
        private string successMessage;
        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategory()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/category");

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<IEnumerable<CategoryDto>>();
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

        public async Task<CategoryDto> GetCategory(Guid categoryId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/category/{categoryId}");

                // Đảm bảo rằng mã trạng thái là thành công (2xx)
                response.EnsureSuccessStatusCode();

                // Đọc và trả về dữ liệu nếu không có lỗi
                return await response.Content.ReadFromJsonAsync<CategoryDto>();
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
                       errorMessage = "NotFound Category";
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
        public string GetErrorMessage()
        {
            return errorMessage;
        }
        public string GetSuccessMessage()
        {
            return successMessage;
        }

        public async Task<bool> CreateCategory(CategoryDto categoryDto)
        {
            try
            {
                // Gửi yêu cầu HTTP POST để tạo mới danh mục
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/category", categoryDto);
                if(response.IsSuccessStatusCode)
                {
                    successMessage = $"Add {categoryDto.CategoryName} Success !";
                    Console.WriteLine($"Success:{response.IsSuccessStatusCode}, Content: {await response.Content.ReadAsStringAsync()}");
                    return true;
                }
                else
                {
                    if(response.StatusCode == HttpStatusCode.Conflict)
                    {
                        
                        errorMessage =  $"{categoryDto.CategoryName} already exists";
                        Console.WriteLine($"Error:{response.StatusCode}, Content: { await response.Content.ReadAsStringAsync()} ");
                        return false;
                    }
                    else if(response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        Console.WriteLine($"Error:{response.StatusCode}, Content: { await response.Content.ReadAsStringAsync()} ");
                        return false;
                    }
                    else if(response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                       Console.WriteLine($"Error:{response.StatusCode}, Content: { await response.Content.ReadAsStringAsync()} ");
                        return false;
                    }
                    else
                    {
                        Console.WriteLine($"Error:{response.StatusCode}, Content: { await response.Content.ReadAsStringAsync()} ");
                        errorMessage = $"Add {categoryDto.CategoryName} Category Fail | Error: {response.StatusCode}";
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

        public async Task<bool> DeleteCategory(Guid categoryId)
        {
            try
            {
                // Gửi yêu cầu HTTP DELETE để xóa danh mục
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/category/{categoryId}");
                if(response.IsSuccessStatusCode)
                {
                    successMessage = $"Delete Category Success !";
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


        public async Task<bool> UpdateCategory(CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                throw new ArgumentNullException(nameof(categoryDto));
            }

            try
            {
                // Chuyển đổi đối tượng DTO thành chuỗi JSON
                string categoryJson = JsonConvert.SerializeObject(categoryDto);
                StringContent content = new StringContent(categoryJson, Encoding.UTF8, "application/json");

                // Gửi yêu cầu HTTP PUT để cập nhật danh mục
                HttpResponseMessage response = await _httpClient.PutAsync($"api/category/{categoryDto.CategoryId}", content);

                if (response.IsSuccessStatusCode)
                {
                    successMessage = $"Update {categoryDto.CategoryName} Success !";
                    Console.WriteLine($"Success:{response.IsSuccessStatusCode}, Content: {await response.Content.ReadAsStringAsync()}");
                    return true;

                }else
                {
                    if(response.StatusCode == HttpStatusCode.Conflict)
                    {
                        errorMessage = $"{categoryDto.CategoryName} already exists";
                        Console.WriteLine($"Error:{response.StatusCode}, Content: { await response.Content.ReadAsStringAsync()} ");
                        return false;
                    }
                    else if(response.StatusCode == HttpStatusCode.BadRequest)
                    {
                       Console.WriteLine($"Error:{response.StatusCode}, Content: { await response.Content.ReadAsStringAsync()} ");
                        return false;
                    }
                    else if(response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                     
                        Console.WriteLine($"Error:{response.StatusCode}, Content: { await response.Content.ReadAsStringAsync()} ");
                        return false;
                    }
                    else
                    {
                        errorMessage = $"Update {categoryDto.CategoryName} Category Fail | Error: {response.StatusCode}";
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

        public string GetErrorMessage(string message)
        {
            throw new NotImplementedException();
        }
    }
}
