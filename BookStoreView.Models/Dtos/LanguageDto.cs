using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreView.Models.Dtos
{
    public class LanguageDto
    {
        [Key]
        public Guid LanguageId { get; set; }

        [Required(ErrorMessage = "Language code is required.")]
        [MaxLength(10, ErrorMessage = "Language code cannot exceed 10 characters.")]
        public string LanguageCode { get; set; }
        [Required(ErrorMessage = "Language name is required.")]
        [MaxLength(50, ErrorMessage = "Language name cannot exceed 50 characters.")]
        public string LanguageName { get; set; }
        public bool IsSelected { get; set; }


    }
}
