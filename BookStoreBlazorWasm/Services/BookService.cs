using BookStoreBlazorWasm.Services.Interfaces;
using BookStoreView.Models.Dtos;
using System.Net.Http.Json;
using System.Net;
using Newtonsoft.Json;
using System.Text;


namespace BookStoreBlazorWasm.Services
{
    public class BookService : IBookService
    {
        private readonly HttpClient _httpClient;
        private string errorMessage;
        private string successMessage;

        public BookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CreateBook(BookDto bookDto)
        {
            if (bookDto == null)
            {
                errorMessage = "The book cannot be null.";
                return false;
            }

            try
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(bookDto), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/book", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    successMessage = $"Add {bookDto.Title} Success !";
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Conflict:
                            errorMessage = $"{bookDto.Title} already exists. Error: {errorContent}";
                            break;
                        case HttpStatusCode.BadRequest:
                            errorMessage = $"Bad request. Error: {errorContent}";
                            break;
                        case HttpStatusCode.InternalServerError:
                            errorMessage = $"Internal server error. Error: {errorContent}";
                            break;
                        default:
                            errorMessage = $"Add {bookDto.Title}  Fail | Error: {response.StatusCode}. Error: {errorContent}";
                            break;
                    }

                    return false;
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request error (e.g., lost connection)

                Console.WriteLine($"HTTP Request Error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteBook(Guid bookId)
        {
            try
            {
                // Gửi yêu cầu HTTP DELETE để xóa danh mục
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/book/{bookId}");
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

        public async Task<BookDto> GetBook(Guid bookId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/book/{bookId}");

                // Đảm bảo rằng mã trạng thái là thành công (2xx)
                response.EnsureSuccessStatusCode();

                // Đọc và trả về dữ liệu nếu không có lỗi
                return await response.Content.ReadFromJsonAsync<BookDto>();
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
                        errorMessage  = "NotFound Supplier";
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

        public async Task<IEnumerable<BookDto>> GetBooks()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/book");

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<IEnumerable<BookDto>>();
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

      

        public async Task<bool> UpdateBook(BookDto bookDto)
        {

            if (bookDto == null)
            {
                throw new ArgumentNullException(nameof(bookDto));
            }

            try
            {
                // Chuyển đổi đối tượng DTO thành chuỗi JSON
                string supplierJson = JsonConvert.SerializeObject(bookDto);
                StringContent content = new StringContent(supplierJson, Encoding.UTF8, "application/json");

                // Gửi yêu cầu HTTP PUT để cập nhật danh mục
                HttpResponseMessage response = await _httpClient.PutAsync($"api/book/{bookDto.BookId}", content);

                if (response.IsSuccessStatusCode)
                {
                    successMessage = $"Update {bookDto.Title} Success !";
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
                        errorMessage = $"Update {bookDto.Title} Category Fail | Error: {response.StatusCode}";
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

   

