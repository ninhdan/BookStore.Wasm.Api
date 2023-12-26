
using BookStoreBlazorWasm.Services.Interfaces;
using BookStoreView.Models.Dtos;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace BookStoreBlazorWasm.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly HttpClient _httpClient;
        private string errorMessage;
        private string successMessage;

        public SupplierService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CreateSupplier(SupplierDto supplierDto)
        {
            try
            {
                // Gửi yêu cầu HTTP POST để tạo mới danh mục
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/supplier", supplierDto);
                if (response.IsSuccessStatusCode)
                {
                    successMessage = $"Add {supplierDto.SupplierName} Success !";
                    Console.WriteLine($"Success:{response.IsSuccessStatusCode}, Content: {await response.Content.ReadAsStringAsync()}");
                    return true;
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest)
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
                        errorMessage = $"Add {supplierDto.SupplierName} Category Fail | Error: {response.StatusCode}";
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

        public async Task<bool> DeleteSupplier(Guid supplierId)
        {
            try
            {
                // Gửi yêu cầu HTTP DELETE để xóa danh mục
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/supplier/{supplierId}");
                if (response.IsSuccessStatusCode)
                {
                    successMessage = $"Delete Supplier Success !";
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

        public async Task<IEnumerable<SupplierDto>> GetAllSupplier()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/supplier");

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<IEnumerable<SupplierDto>>();
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

        public string GetErrorMessage()
        {
            return errorMessage;
        }

        public string GetSuccessMessage()
        {
            return successMessage;
        }

        public async Task<SupplierDto> GetSupplier(Guid supplierId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/supplier/{supplierId}");

                // Đảm bảo rằng mã trạng thái là thành công (2xx)
                response.EnsureSuccessStatusCode();

                // Đọc và trả về dữ liệu nếu không có lỗi
                return await response.Content.ReadFromJsonAsync<SupplierDto>();
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
                       errorMessage = "NotFound Supplier";
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

        public async Task<bool> UpdateSupplier(SupplierDto supplierDto)
        {
            if (supplierDto == null)
            {
                throw new ArgumentNullException(nameof(supplierDto));
            }

            try
            {
                // Chuyển đổi đối tượng DTO thành chuỗi JSON
                string supplierJson = JsonConvert.SerializeObject(supplierDto);
                StringContent content = new StringContent(supplierJson, Encoding.UTF8, "application/json");

                // Gửi yêu cầu HTTP PUT để cập nhật danh mục
                HttpResponseMessage response = await _httpClient.PutAsync($"api/supplier/{supplierDto.SupplierId}", content);

                if (response.IsSuccessStatusCode)
                {
                    successMessage = $"Update {supplierDto.SupplierName} Success !";
                    Console.WriteLine($"Success:{response.IsSuccessStatusCode}, Content: {await response.Content.ReadAsStringAsync()}");
                    return true;

                }
                else
                {
                   
                   if (response.StatusCode == HttpStatusCode.BadRequest)
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
                        errorMessage = $"Update {supplierDto.SupplierName} Category Fail | Error: {response.StatusCode}";
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
