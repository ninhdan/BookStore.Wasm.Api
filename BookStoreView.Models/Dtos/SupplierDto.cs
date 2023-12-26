using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreView.Models.Dtos
{
    public class SupplierDto
    {
        [Key]
        public Guid SupplierId { get; set; }
        [Required(ErrorMessage = "Supplier name is required.")]
        [MaxLength(50, ErrorMessage = "Supplier name cannot exceed 50 characters.")]
        public string SupplierName { get; set; }
        public bool IsSelected { get; set; }

    }
}
