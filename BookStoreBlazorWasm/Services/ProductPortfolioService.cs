using BookStoreBlazorWasm.Services.Interfaces;
using BookStoreView.Models.Dtos.DtoProductPortfolio;
using System.Net.Http;
using System.Net.Http.Json;

namespace BookStoreBlazorWasm.Services
{
    public class ProductPortfolioService : IProductPortfolioService
    {
        private readonly HttpClient httpClient;

        public ProductPortfolioService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }


        public async Task<List<CategoryDtopp>> GetCategoriesByProductPortfolioAsync(bool productPortfolio)
        {
            return await httpClient.GetFromJsonAsync<List<CategoryDtopp>>($"api/productportfolio/byProductPortfolio/{productPortfolio}");
        }

        public async Task<List<SubCategoryDtopp>> GetSubCategoriesByCategoryAsync(Guid categoryId)
        {
            return await httpClient.GetFromJsonAsync<List<SubCategoryDtopp>>($"api/productportfolio/{categoryId}/subcategories");
        }

        public async Task<List<BookDtopp>> GetBooksBySubCategory(Guid subCategoryId)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<List<BookDtopp>>($"api/productportfolio/subcategories/{subCategoryId}/books");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

    }
}
