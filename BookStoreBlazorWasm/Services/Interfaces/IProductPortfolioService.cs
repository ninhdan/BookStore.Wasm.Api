using BookStoreView.Models.Dtos.DtoProductPortfolio;

namespace BookStoreBlazorWasm.Services.Interfaces
{
    public interface IProductPortfolioService
    {
        Task<List<CategoryDtopp>> GetCategoriesByProductPortfolioAsync(bool productPortfolio);
        Task<List<SubCategoryDtopp>> GetSubCategoriesByCategoryAsync(Guid categoryId);

        Task<List<BookDtopp>> GetBooksBySubCategory(Guid subCategoryId);
    }
}
