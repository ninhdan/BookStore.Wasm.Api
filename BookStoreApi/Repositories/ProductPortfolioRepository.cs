using AutoMapper;
using BookStoreApi.Models;
using BookStoreApi.Repositories.Interfaces;
using BookStoreView.Models.Dtos.DtoProductPortfolio;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Repositories
{
    public class ProductPortfolioRepository : IProductPortfolioRepository
    {
        private readonly DbWater7Context context;
        private readonly IMapper mapper;

        public ProductPortfolioRepository(DbWater7Context context , IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<BookDtopp>> GetBooksBySubCategoryAsync(Guid subCategoryId)
        {
            var books = await context.Books
             .Where(b => b.SubcategoryId == subCategoryId)
             .ToListAsync();

            return mapper.Map<List<BookDtopp>>(books);
        }

        public async Task<List<CategoryDtopp>> GetCategoriesByProductPortfolioAsyn(bool productPortfolio)
        {
            var categories = await context.Categories
           .Where(c => c.ProductPortfolio == productPortfolio)
           .Include(c => c.SubCategories)
           .ToListAsync();

            return mapper.Map<List<CategoryDtopp>>(categories);
        }

        public async Task<List<SubCategoryDtopp>> GetSubCategoriesByCategoryAsync(Guid categoryId)
        {
            var subCategories = await context.SubCategories
             .Where(sc => sc.CategoryId == categoryId)
             .ToListAsync();

            return mapper.Map<List<SubCategoryDtopp>>(subCategories);
        }


    }
}
