using BookStoreApi.Models;
using BookStoreView.Models.Dtos.ShoppingCart;

namespace BookStoreApi.Repositories.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto, Guid userId);
        Task<CartItem> UpdateQuanlity(Guid id, CartIemQuanlityUpdateDto cartIemQuanlityUpdateDto);
        Task<CartItem> DeleteItem(Guid id);

        Task<CartItem> GetItem(Guid id);
        Task<IEnumerable<CartItem>> GetItems(Guid customerId);




    }
}
