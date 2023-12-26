using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreView.Models.Dtos.DtoUser
{
    public class UpdateUserDto
    {
        public Guid UserId { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập họ")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập tên")]
        public string Lastname { get; set; }

        [EmailAddress(ErrorMessage = "Địa chỉ gmail không đúng")]
        public string? Email { get; set; }
        public bool? Gender { get; set; }
        [DataType(DataType.Date, ErrorMessage = "Ngày sinh không đúng định dạng")]
        public DateTime? Birthday { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không đúng.")]
        public string Phone { get; set; }

    }
}
