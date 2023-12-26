using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreView.Models.Dtos.ShoppingCart
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }

        public DateTime DateOrder { get; set; }

        public string? Note { get; set; }

        public decimal Grandtotal { get; set; }

        public decimal Feeshipp { get; set; }

        public Guid UserId { get; set; }
    }
}
