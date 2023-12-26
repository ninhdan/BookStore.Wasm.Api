using BookStoreView.Models.Dtos.DtoProductPortfolio;

namespace BookStoreApi.Repositories.Interfaces
{
    public interface IProductPortfolioRepository
    {
        Task<List<CategoryDtopp>> GetCategoriesByProductPortfolioAsyn(bool productPortfolio);
        Task<List<SubCategoryDtopp>> GetSubCategoriesByCategoryAsync(Guid categoryId);

        Task<List<BookDtopp>> GetBooksBySubCategoryAsync(Guid subCategoryId);
    }
}
