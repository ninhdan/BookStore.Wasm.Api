using BookStoreApi.Controllers;
using BookStoreApi.Models;
using BookStoreView.Models.Dtos.DtoUser;
using BookStoreView.Models.Dtos.ShoppingCart;
using Microsoft.AspNetCore.Identity;

namespace BookStoreApi.Extensions
{
    public static class DtoConversions
    {

        public static IEnumerable<CartItemDto> ConvertToDto(this IEnumerable<CartItem> cartItems, IEnumerable<Book> books)
        {
            return (from cartItem in cartItems
                    join book in books
                    on cartItem.BookId equals book.BookId
                    select new CartItemDto
                    {
                        ItemId = cartItem.ItemId,
                        BookId = cartItem.BookId,
                        Title = book.Title,
                        PathImage = book.PathImage,
                        Price = book.Price,
                        OrderId = cartItem.OrderId,
                        Quanlity = cartItem.Quanlity,
                        Grandtotal = book.Price * cartItem.Quanlity
                    }).ToList();


        }


        public static CartItemDto ConvertToDto(this CartItem cartItem, Book book)
        {
            return  new CartItemDto
                    {
                        ItemId = cartItem.ItemId,
                        BookId = cartItem.BookId,
                        Title = book.Title,
                        PathImage = book.PathImage,
                        Price = book.Price,
                        OrderId = cartItem.OrderId,
                        Quanlity = cartItem.Quanlity,
                        Grandtotal = book.Price * cartItem.Quanlity
                    };


        }




        public static UpdateUserDto ConvertToDto(User user) {

            return new UpdateUserDto
            {
                UserId = user.UserId,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                Gender = user.Gender,
                Birthday = user.Birthday,
                Phone = user.Phone,
            };
        }

     
    }
}
