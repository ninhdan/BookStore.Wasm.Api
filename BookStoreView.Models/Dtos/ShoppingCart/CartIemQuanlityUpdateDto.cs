using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreView.Models.Dtos.ShoppingCart
{
    public class CartIemQuanlityUpdateDto
    {
        public Guid CartItemId { get; set; }

        public int Quanlity { get; set; }

    }
}
