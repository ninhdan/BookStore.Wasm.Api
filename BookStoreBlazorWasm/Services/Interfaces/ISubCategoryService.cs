using BookStoreView.Models.Dtos;

namespace BookStoreBlazorWasm.Services.Interfaces
{
    public interface ISubCategoryService
    {
        Task<IEnumerable<SubCategoryDto>> GetAllSubCategory();
        public string GetErrorMessage();
        public string GetSuccessMessage();
        Task<SubCategoryDto> GetSubCategory(Guid subCategoryId);
        Task<bool> CreateSubCategory(SubCategoryDto subcategoryDto);
        Task<bool> UpdateSubCategory(SubCategoryDto subcategoryDto);
        Task<bool> DeleteSubCategory(Guid subCategoryId);




    }
}
