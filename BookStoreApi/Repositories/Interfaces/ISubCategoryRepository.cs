using BookStoreApi.Models;
using BookStoreView.Models.Dtos;

namespace BookStoreApi.Repositories.Interfaces
{
    public interface ISubCategoryRepository
    {
        ICollection<SubCategory> GetAllSubCategory();
        SubCategory GetSubCategory(Guid SubCategoryId);
        List<SubCategoryDto> GetSubCategoriesWithCategory();
        bool SubCategoryExists(Guid SubCategoryId);
        bool SubCategoryNameExists(string SubCategoryName);
        bool CreateSubCategory(SubCategory SubCategory);
        bool UpdateSubCategory(SubCategory SubCategory);
        bool DeleteSubCategory(SubCategory SubCategory);
        bool Save();







    }
}
