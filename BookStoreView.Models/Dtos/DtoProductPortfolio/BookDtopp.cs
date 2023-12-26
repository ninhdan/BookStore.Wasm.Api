using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreView.Models.Dtos.DtoProductPortfolio
{
    public class BookDtopp
    {
        public Guid BookId { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }
        public string? PathImage { get; set; }
    }
}
