using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreView.Models.Dtos.DtoUser
{
    public class AddressDto
    {
        public Guid AddressId { get; set; }

        public string StreetNumber { get; set; } 

        public string Ward { get; set; } 

        public string City { get; set; } 

        public string Province { get; set; } 

        public string Country { get; set; }

        public Guid UserId { get; set; }

        public bool StatusAddress { get; set; }
    }
}
