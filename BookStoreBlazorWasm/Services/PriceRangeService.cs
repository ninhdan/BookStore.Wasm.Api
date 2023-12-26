using Blazored.Toast.Services;
using BookStoreBlazorWasm.Services.Interfaces;
using BookStoreView.Models.Dtos;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace BookStoreBlazorWasm.Services
{
    public class PriceRangeService : IPriceRangeService
    {
        private readonly HttpClient _client;
        private string errorMessage;
        private string successMessage;
        public PriceRangeService(HttpClient client)
        {
            _client = client;
        }


        public async Task<bool> CreatePriceRange(PriceRangeDto priceRangeDto)
        {
            try
            {
                var repsonse = await _client.PostAsJsonAsync("api/pricerange", priceRangeDto);
              
                if (repsonse.IsSuccessStatusCode)
                {
                    successMessage = $"Add {priceRangeDto.PriceRangeName} Success !";
                    Console.WriteLine($"Success:{repsonse.IsSuccessStatusCode}, Content: {await repsonse.Content.ReadAsStringAsync()}");
                    return true;
                }
                else
                {
                    if (repsonse.StatusCode == HttpStatusCode.Conflict)
                    {

                        errorMessage = $"{priceRangeDto.PriceRangeName} already exists";
                        Console.WriteLine($"Error:{repsonse.StatusCode}, Content: {await repsonse.Content.ReadAsStringAsync()} ");
                        return false;
                    }
                    else if (repsonse.StatusCode == HttpStatusCode.BadRequest)
                    {
                        Console.WriteLine($"Error:{repsonse.StatusCode}, Content: {await repsonse.Content.ReadAsStringAsync()} ");
                        return false;
                    }
                    else if (repsonse.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        Console.WriteLine($"Error:{repsonse.StatusCode}, Content: {await repsonse.Content.ReadAsStringAsync()} ");
                        return false;
                    }
                    else
                    {
                        Console.WriteLine($"Error:{repsonse.StatusCode}, Content: {await repsonse.Content.ReadAsStringAsync()} ");
                        errorMessage = $"Add {priceRangeDto.PriceRangeName} Category Fail | Error: {repsonse.StatusCode}";
                        return false;
                    }
                }

            }catch (HttpRequestException ex)
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

        public async Task<bool> DeletePriceRange(Guid priceRangeId)
        {
            try
            {
                // Gửi yêu cầu HTTP DELETE để xóa danh mục
                HttpResponseMessage response = await _client.DeleteAsync($"api/pricerange/{priceRangeId}");
                if (response.IsSuccessStatusCode)
                {
                    successMessage = $"Delete Price Range Success !";
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

        public async Task<PriceRangeDto> GetPriceRange(Guid priceRangeId)
        {
            try
            {
                var response = await _client.GetAsync($"api/pricerange/{priceRangeId}");

                // Đảm bảo rằng mã trạng thái là thành công (2xx)
                response.EnsureSuccessStatusCode();

                // Đọc và trả về dữ liệu nếu không có lỗi
                return await response.Content.ReadFromJsonAsync<PriceRangeDto>();
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
                        errorMessage = "NotFound price range";
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

        public async Task<IEnumerable<PriceRangeDto>> GetPriceRanges()
        {
            try
            {
                var response = await _client.GetAsync("api/pricerange");

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<IEnumerable<PriceRangeDto>>();
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

        public string GetSuccessMessage()
        {
            return successMessage;
        }

        public async Task<bool> UpdatePriceRange(PriceRangeDto priceRangeDto)
        {
            if (priceRangeDto == null)
            {
                throw new ArgumentNullException(nameof(priceRangeDto));
            }

            try
            {
                // Chuyển đổi đối tượng DTO thành chuỗi JSON
                string supplierJson = JsonConvert.SerializeObject(priceRangeDto);
                StringContent content = new StringContent(supplierJson, Encoding.UTF8, "application/json");

                // Gửi yêu cầu HTTP PUT để cập nhật danh mục
                HttpResponseMessage response = await _client.PutAsync($"api/pricerange/{priceRangeDto.PriceRangeId}", content);

                if (response.IsSuccessStatusCode)
                {
                    successMessage = $"Update {priceRangeDto.PriceRangeName} Success !";
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
                        errorMessage = $"Update {priceRangeDto.PriceRangeName} Category Fail | Error: {response.StatusCode}";
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
