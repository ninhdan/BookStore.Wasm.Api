using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreView.Models.Dtos.DtoUser
{
    public class LoginUserDto
    {

        [Required(ErrorMessage = " Bạn chưa nhập tài khoản")]
        public string Phone { get; set;}

        [Required(ErrorMessage = " Bạn chưa nhập mật khẩu")]
        public string Password { get; set;}

    }
}
