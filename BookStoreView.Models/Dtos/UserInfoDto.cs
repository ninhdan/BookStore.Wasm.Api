using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreView.Models.Dtos
{
    public class UserInfoDto
    {
        public Guid UserId { get; set; }
        public string fistName { get; set; }
        public string lastName { get; set; }
        public string Phone { get; set; }
    }
}
