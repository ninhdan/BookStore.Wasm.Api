using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreView.Models.Dtos.DtoUser
{
    public class UserDto
    {
        public Guid UserId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; } 

        public string? Email { get; set; }

        public bool? Gender { get; set; }

        public DateTime? Birthday { get; set; }

        public string Phone { get; set; } 

        public string Password { get; set; }

        public bool Accountstatus { get; set; } 

        public string Role { get; set; } 
        public List<AddressDto> Addresses { get; set; }
    }
}
