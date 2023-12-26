using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace BookStoreView.Models.Dtos
{
    public class CategoryDto
    {
        [Key]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [MaxLength(50, ErrorMessage = "Category name cannot exceed 50 characters.")]
        
        public string CategoryName { get; set; }
      
        public bool ProductPortfolio { get; set; }

        //public List<SubCategoryDto> SubCategories { get; set; }
        public bool IsSelected { get; set; }
    }
}
