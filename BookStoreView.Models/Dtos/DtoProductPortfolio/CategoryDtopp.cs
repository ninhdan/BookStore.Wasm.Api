using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreView.Models.Dtos.DtoProductPortfolio
{
    public class CategoryDtopp
    {

        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<SubCategoryDtopp> SubCategories { get; set; }


    }
}
