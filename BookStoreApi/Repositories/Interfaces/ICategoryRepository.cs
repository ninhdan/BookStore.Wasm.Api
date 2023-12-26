using BookStoreApi.Models;

namespace BookStoreApi.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetAllCategory();
        Category GetCategory(Guid categoryId);
        ICollection<SubCategory> GetCategoriesOfSubCategories(Guid subCategoryId);
        bool CategoryExists(Guid categoryId);
        bool CategoryNameExists(string categoryName);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();
       
    }
}
