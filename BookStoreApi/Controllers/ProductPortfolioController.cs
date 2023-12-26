using BookStoreApi.Repositories;
using BookStoreApi.Repositories.Interfaces;
using BookStoreView.Models.Dtos.DtoProductPortfolio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductPortfolioController : ControllerBase
    {
        private readonly IProductPortfolioRepository productPortfolioRepository;

        public ProductPortfolioController(IProductPortfolioRepository productPortfolioRepository)
        {
            this.productPortfolioRepository = productPortfolioRepository;
        }

        [HttpGet("byProductPortfolio/{productPortfolio}")]
        public async Task<ActionResult<List<CategoryDtopp>>> GetCategoriesByProductPortfolio(bool productPortfolio)
        {
            var categories = await productPortfolioRepository.GetCategoriesByProductPortfolioAsyn(productPortfolio);
            return Ok(categories);
        }

        [HttpGet("{categoryId:Guid}/subcategories")]
        public async Task<ActionResult<List<SubCategoryDtopp>>> GetSubCategories(Guid categoryId)
        {
            var subCategories = await productPortfolioRepository.GetSubCategoriesByCategoryAsync(categoryId);
            return Ok(subCategories);
        }

        [HttpGet("subcategories/{subCategoryId}/books")]
        public async Task<ActionResult<List<BookDtopp>>> GetBooksBySubCategory(Guid subCategoryId)
        {
            try
            {
                var books = await productPortfolioRepository.GetBooksBySubCategoryAsync(subCategoryId);

                return Ok(books);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về 500 Internal Server Error
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }



    }
}
