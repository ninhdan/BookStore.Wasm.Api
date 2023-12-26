using BookStoreView.Models.Dtos.ShoppingCart;

namespace BookStoreBlazorWasm.Services.Interfaces
{
    public interface IShoppingCartService
    {
        Task<List<CartItemDto>> GetItems(Guid customerId);
        Task<CartItemDto> DeleteItem(Guid id);
        Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto, Guid userId);
        Task<CartItemDto> UpdateQuantity(CartIemQuanlityUpdateDto cartIemQuanlityUpdateDto);

        event Action<int> OnShoppingCartChanged;

        void RaiseEventOnShoppingCartChanged(int totalQty);
    }
}
