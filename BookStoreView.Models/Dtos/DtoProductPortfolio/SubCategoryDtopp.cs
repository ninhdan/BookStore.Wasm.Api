using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreView.Models.Dtos.DtoProductPortfolio
{
    public class SubCategoryDtopp
    {

        public Guid SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }

        public List<BookDtopp> Books { get; set; }

    }
}
