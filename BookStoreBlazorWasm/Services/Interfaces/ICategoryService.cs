using BookStoreView.Models.Dtos;


namespace BookStoreBlazorWasm.Services.Interfaces
{
    public interface ICategoryService 
    {
        Task<IEnumerable<CategoryDto>> GetAllCategory();
        public string GetErrorMessage();
        public string GetSuccessMessage();
        Task<CategoryDto> GetCategory(Guid categoryId);
        Task<bool> CreateCategory(CategoryDto categoryDto);
        Task<bool> UpdateCategory(CategoryDto categoryDto);
        Task<bool> DeleteCategory(Guid categoryId);
       
    }
}
