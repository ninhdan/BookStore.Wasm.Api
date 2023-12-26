using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreView.Models.Dtos.ShoppingCart
{
    public class CartItemToAddDto
    {

        public Guid BookId { get; set; }

        public Guid OrderId { get; set; }

        public int Quanlity { get; set; }

    }
}
