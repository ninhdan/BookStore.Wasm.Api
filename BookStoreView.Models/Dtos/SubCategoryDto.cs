using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreView.Models.Dtos
{
    public class SubCategoryDto
    {
        [Key]
        public Guid SubCategoryId { get; set; }

        [Required(ErrorMessage = "SubCategory name is required")]
        [MaxLength(50, ErrorMessage = "SubCategory name cannot exceed 50 characters.")]
        public string SubCategoryName { get; set; } 
        [Required(ErrorMessage = "Category is required")]
        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; }

        public bool IsSelected { get; set; }

    }
}
