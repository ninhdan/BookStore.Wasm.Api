using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreView.Models.Dtos.ShoppingCart
{
    public class BookDtoIndex
    {
            public Guid BookId { get; set; }
            [Required(ErrorMessage = "Title is required")]
            [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
            public string? Title { get; set; }

            public string? Author { get; set; }

            [Required(ErrorMessage = "Publisher is required")]
            [MaxLength(100, ErrorMessage = "Publisher cannot exceed 100 characters")]
            public string? Publisher { get; set; }


            [Required(ErrorMessage = "Publisher year is required")]
            [ValidateDate(ErrorMessage = "Publisher year must be less than or equal to current year")]
            public DateTime PublisherYear { get; set; }


            [Required(ErrorMessage = "Size is required")]
            [MaxLength(30, ErrorMessage = "Size cannot exceed 30 characters")]
            public string? Size { get; set; }

            [MaxLength(100, ErrorMessage = "Translator cannot exceed 100 characters")]

            public string? Translator { get; set; }

            [Required(ErrorMessage = "Weight is required")]
            [Range(1, int.MaxValue, ErrorMessage = "Weight must be > 0")]
            public int Weight { get; set; }

            [Required(ErrorMessage = "Quanlity page is required")]
            [Range(1, int.MaxValue, ErrorMessage = "Quanlity page must be > 0")]
            public int QuanlityPage { get; set; }

            public string? Description { get; set; }

            [Required(ErrorMessage = "Supplier is required")]

            public Guid SupplierId { get; set; }
            [NotMapped]
            public string? SupplierName { get; set; }
            [Required(ErrorMessage = "Language is required")]
            public Guid LanguageId { get; set; }
            [NotMapped]
            public string? LanguageName { get; set; }

            [Required(ErrorMessage = "Subcategory is required")]
            public Guid SubcategoryId { get; set; }
            [NotMapped]
            public string? SubcategoryName { get; set; }

            [Required(ErrorMessage = "Layout is required")]
            public Guid LayoutId { get; set; }
            [NotMapped]
            public string? LayoutName { get; set; }

            [Required(ErrorMessage = "Price range is required")]
            public Guid PriceRangeId { get; set; }
            [NotMapped]
            public string? PriceRangeName { get; set; }

            [Required(ErrorMessage = "Price is required")]
            [Range(1, int.MaxValue, ErrorMessage = "Price must be > 0")]
            public decimal Price { get; set; }
            public string? PathImage { get; set; }


            public bool IsSelected { get; set; }


        }

        public class ValidateDateAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                DateTime dateTimeValue = (DateTime)value;

                if (dateTimeValue > DateTime.Now)
                {
                    return new ValidationResult(ErrorMessage);
                }

                return ValidationResult.Success;
            }
        }


}
