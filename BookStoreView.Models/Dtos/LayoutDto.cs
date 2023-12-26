using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreView.Models.Dtos
{
    public class LayoutDto
    {
        [Key]
        public Guid LayoutId { get; set; }
        [Required(ErrorMessage = "Layout name is required.")]
        [MaxLength(50, ErrorMessage = "Layout name cannot exceed 50 characters.")]
        public string LayoutName { get; set; }

        public bool IsSelected { get; set; }

    }
}
