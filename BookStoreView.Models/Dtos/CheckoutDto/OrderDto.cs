using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreView.Models.Dtos.CheckoutDto
{
    public class OrderDto
    {
        //-----------CART ITEM---------------//
        public Guid ItemId { get; set; }

        public Guid BookId { get; set; }

        public int Quanlity { get; set; }

        //-----------------USER-------------------//
        public Guid UserId { get; set; }
        public string Firstname { get; set; } 

        public string Lastname { get; set; } 

        public string Phone { get; set; }

        //---------------ORDERS-----------------------//

        public Guid OrderId { get; set; }

        public DateTime? DateOrder { get; set; }

        public string? Note { get; set; }

        public decimal? Grandtotal { get; set; }

        public decimal? Feeshipp { get; set; }

        public bool Issubmitted { get; set; }

        //------------------ADDRESS----------------------//

        public Guid AddressId { get; set; }
        public string StreetNumber { get; set; }

        public string Ward { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string Country { get; set; }

        public bool StatusAddress { get; set; }








    }
}
