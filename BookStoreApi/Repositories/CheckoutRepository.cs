using BookStoreApi.Models;
using BookStoreApi.Repositories.Interfaces;
using BookStoreView.Models.Dtos.CheckoutDto;
using BookStoreView.Models.Dtos.ShoppingCart;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Repositories
{
    public class CheckoutRepository : ICheckOutRepository
    {
        private readonly DbWater7Context _context;
        private readonly DbWater7Context content;

        public CheckoutRepository(DbWater7Context content)
        {
            this.content = content;
        }
        public async Task<BookStoreView.Models.Dtos.CheckoutDto.OrderDto> GetCartItem(Guid UserId)
        {
            return await (from order in this._context.Orders
                          join cartItem in this._context.CartItems on order.OrderId equals cartItem.OrderId
                          join user in this._context.Users on order.UserId equals user.UserId
                          join address in this._context.Addresses on order.UserId equals address.UserId
                          where order.UserId == UserId && address.StatusAddress == true
                          select new BookStoreView.Models.Dtos.CheckoutDto.OrderDto
                          {
                              ItemId = cartItem.ItemId,
                              OrderId = order.OrderId,
                              BookId = cartItem.BookId,
                              AddressId = address.AddressId,
                              Quanlity = cartItem.Quanlity,
                              DateOrder = order.DateOrder,
                              Firstname = user.Firstname,
                              Lastname = user.Lastname,
                              Phone = user.Phone,
                              StreetNumber = address.StreetNumber,
                              Ward = address.Ward,
                              City = address.City,
                              Province = address.Province,
                              Country = address.Country,
                              StatusAddress = address.StatusAddress,
                              Note = order.Note,
                              Grandtotal = order.Grandtotal,
                              Feeshipp = order.Feeshipp,
                              Issubmitted = order.Issubmitted,
                          }).SingleOrDefaultAsync<BookStoreView.Models.Dtos.CheckoutDto.OrderDto>();
        }

        public async Task<IEnumerable<Order>> GetCartItems(Guid UserId)
        {
            return await (from order in this._context.Orders
                          join cartItem in this._context.CartItems on order.OrderId equals cartItem.OrderId
                          join user in this._context.Users on order.UserId equals user.UserId
                          join address in this._context.Addresses on order.UserId equals address.UserId
                          where order.UserId == UserId && address.StatusAddress == true
                          select new Order
                          {
                             
                              OrderId = cartItem.OrderId,
                              DateOrder = order.DateOrder,
                              Note = order.Note,
                              Grandtotal = order.Grandtotal,
                              Feeshipp = order.Feeshipp,
                              Issubmitted = order.Issubmitted,
                          }).ToArrayAsync();
        }

      




    }
}
