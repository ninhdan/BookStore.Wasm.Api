
using BookStoreApi.Models;
using BookStoreApi.Repositories.Interfaces;
using BookStoreView.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Repositories
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly DbWater7Context _context;

        public SubCategoryRepository(DbWater7Context context)
        {
            _context = context;
        }


        public bool CreateSubCategory(SubCategory SubCategory)
        {
            _context.Add(SubCategory);
            return Save();
        }

        public bool DeleteSubCategory(SubCategory SubCategory)
        {
            _context.Remove(SubCategory);
            return Save();
        }

        public ICollection<SubCategory> GetAllSubCategory()
        {
            return _context.SubCategories.ToList(); 
        }

        public List<SubCategoryDto> GetSubCategoriesWithCategory()
        {
            var subCategoryDtos = _context.SubCategories
           .Select(subCategory => new SubCategoryDto
           {
               SubCategoryId = subCategory.SubcategoryId,
               SubCategoryName = subCategory.SubcategoryName,
               CategoryId = subCategory.CategoryId,
               CategoryName = subCategory.Category.CategoryName // Assign CategoryName directly
           })
           .ToList();

            return subCategoryDtos;
        }

        public SubCategory GetSubCategory(Guid SubCategoryId)
        {
            return _context.SubCategories.Where(c => c.SubcategoryId == SubCategoryId).FirstOrDefault();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }

        public bool SubCategoryExists(Guid SubCategoryId)
        {
            return _context.SubCategories.Any(c => c.SubcategoryId == SubCategoryId);
            
        }

        public bool SubCategoryNameExists(string SubCategoryName)
        {
            return _context.SubCategories.All(c => c.SubcategoryName.ToLower().Trim() == SubCategoryName.ToLower().Trim());
        }

        public bool UpdateSubCategory(SubCategory SubCategory)
        {
            _context.Update(SubCategory);
            return Save();
        }
    }
}
