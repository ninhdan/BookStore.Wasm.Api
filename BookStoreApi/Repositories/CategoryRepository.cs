
using BookStoreApi.Models;
using BookStoreApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DbWater7Context _context;
        public CategoryRepository(DbWater7Context context)
        {
            _context = context;
        }

        public ICollection<Category> GetAllCategory()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(Guid categoryId)
        {
            return _context.Categories.Where(c => c.CategoryId == categoryId).FirstOrDefault();
        }

        public bool CategoryExists(Guid categoryId)
        {
            return _context.Categories.Any(c => c.CategoryId == categoryId);
        }

        public bool CategoryNameExists(string categoryName)
        {
            return _context.Categories.Any(c => c.CategoryName.ToLower().Trim() == categoryName.ToLower().Trim());
        }


        public bool CreateCategory(Category category)
        {
            _context.Add(category);
            return Save();
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();
        }

       

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public ICollection<SubCategory> GetCategoriesOfSubCategories(Guid subCategoryId)
        {
            SubCategory targetSubCategory = _context.SubCategories.Where(s => s.SubcategoryId == subCategoryId).FirstOrDefault();

            return targetSubCategory != null
                ? _context.SubCategories.Where(s => s.CategoryId == targetSubCategory.CategoryId).ToList()
                : (ICollection<SubCategory>)new List<SubCategory>();
        }


    }
}
