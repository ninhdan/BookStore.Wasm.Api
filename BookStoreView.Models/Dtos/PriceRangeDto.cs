using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreView.Models.Dtos
{
    public class PriceRangeDto
    {
        [Key]
        public Guid PriceRangeId { get; set; }

        [Required(ErrorMessage = "Price Range name is required.")]
        [MaxLength(50, ErrorMessage = "Price Range name cannot exceed 50 characters.")]
        public string PriceRangeName { get; set; }

        [Required(ErrorMessage = "Max Price is required.")]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Min Price must be greater than or equal to 0.")]
        public decimal MinPrice { get; set; }

        [Required(ErrorMessage = "Max Price is required.")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Max Price must be greater than or equal to Min Price.")]
        public decimal MaxPrice { get; set; }

        public bool IsSelected { get; set; }



    }
}
