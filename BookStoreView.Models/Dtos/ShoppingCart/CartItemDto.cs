using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreView.Models.Dtos.ShoppingCart
{
    public class CartItemDto
    {
        public Guid ItemId { get; set; }

        public int Quanlity { get; set; }

        public Guid BookId { get; set; }

        public Guid OrderId { get; set; }

        public string Title { get; set; }

        public string PathImage { get; set; }

        public decimal Grandtotal { get; set; }

        public decimal Price { get; set; }


    }
}
