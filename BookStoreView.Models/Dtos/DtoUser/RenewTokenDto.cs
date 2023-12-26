using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreView.Models.Dtos.DtoUser
{
    public class RenewTokenDto
    {
        public Guid UserId { get; set; }
        public string RefreshToken { get; set; }


    }
}
