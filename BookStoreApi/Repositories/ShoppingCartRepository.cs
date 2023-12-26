
using BookStoreApi.Models;
using BookStoreApi.Repositories.Interfaces;
using BookStoreView.Models.Dtos.ShoppingCart;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly DbWater7Context _context;

        public ShoppingCartRepository(DbWater7Context context)
        {
            _context = context;

        }


        private async Task<bool> CartItemExists( Guid bookId,Guid orderId)
        {
            return await this._context.CartItems.AnyAsync(c => c.OrderId == orderId &&
                                                                     c.BookId == bookId);
        }

        public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto , Guid userId)
        {

            var order = await _context.Orders.FirstOrDefaultAsync(o => o.UserId == userId && !o.Issubmitted);

            if (order == null)
            {
                order = new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = userId,
                    Issubmitted = false
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
            }



            if (await CartItemExists( cartItemToAddDto.BookId, cartItemToAddDto.OrderId) == false)
            {
                var item = await (from book in this._context.Books
                                  where book.BookId == cartItemToAddDto.BookId

                                  select new CartItem
                                  {
                                      BookId = book.BookId,
                                      Quanlity = cartItemToAddDto.Quanlity,
                                      OrderId = order.OrderId,

                                  }).SingleOrDefaultAsync();


                if (item != null)
                {
                    var result = await this._context.CartItems.AddAsync(item);
                    await this._context.SaveChangesAsync();
                    return result.Entity;
                }
            }
            return null;
        }

        public async Task<CartItem> DeleteItem(Guid id)
        {
            var item = await this._context.CartItems.FindAsync(id);
            if (item != null)
            {
                this._context.CartItems.Remove(item);
                await this._context.SaveChangesAsync();
                
            }
            return item;
        }

        public async Task<CartItem> GetItem(Guid id)
        {
            return await (from order in this._context.Orders
                          join cartItem in this._context.CartItems
                          on order.OrderId equals cartItem.OrderId
                          where cartItem.ItemId == id
                          select new CartItem
                          {
                              ItemId = cartItem.ItemId,
                              BookId = cartItem.BookId,
                              Quanlity = cartItem.Quanlity,
                              OrderId = cartItem.OrderId
                          }).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<CartItem>> GetItems(Guid CustomerId)
        {
            return await (from order in this._context.Orders
                          join cartItem in this._context.CartItems
                          on order.OrderId equals cartItem.OrderId
                          where order.UserId == CustomerId

                          select new CartItem
                          {
                              ItemId = cartItem.ItemId,
                              OrderId = order.OrderId,
                              BookId = cartItem.BookId,
                              Quanlity = cartItem.Quanlity
                          }).ToListAsync();
        }

        public async Task<CartItem> UpdateQuanlity(Guid id, CartIemQuanlityUpdateDto cartIemQuanlityUpdateDto)
        {
            var item = await this._context.CartItems.FindAsync(id);
            if (item != null)
            {
                item.Quanlity = cartIemQuanlityUpdateDto.Quanlity;
                await this._context.SaveChangesAsync();
                return item;

            }

            return null;
        }

     
    }
}
