using BookStoreBlazorWasm.Services.Interfaces;
using BookStoreView.Models.Dtos.ShoppingCart;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace BookStoreBlazorWasm.Pages.SellingBooks
{
    public class ShoppingCartBase : ComponentBase
    {

        [Inject]  IShoppingCartService _shoppingCartService { get; set; }

        [Inject] AuthenticationStateProvider AuthProvider { get; set; }

        [Inject] NavigationManager navigationManager { get; set; }

        public List<CartItemDto> shoppingCartItems { get; set; }

        public string ErrorMessage { get; set; }





        protected string Grandtotal { get; set; }
        protected int TotalQuantily { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try
            {

                await LoadingShoppingCart();
               
            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }

        private async Task LoadingShoppingCart()
        {
            var userId = await GetUserIdAsync();
            if (userId.HasValue)
            {
                shoppingCartItems = await _shoppingCartService.GetItems(userId.Value);
                CartChanged();
            }
            else
            {
                var returnUrl = navigationManager.Uri;

               
                if (Uri.IsWellFormedUriString(returnUrl, UriKind.RelativeOrAbsolute))
                {
                   
                    var appUri = new Uri(navigationManager.BaseUri);
                    var returnUri = new Uri(returnUrl, UriKind.RelativeOrAbsolute);

                    if (!returnUri.IsAbsoluteUri || returnUri.Authority == appUri.Authority)
                    {
                        
                        navigationManager.NavigateTo($"/login?returnUrl={Uri.EscapeDataString(returnUrl)}", true);
                    }
                  
                }
               

            }
        }


        private async Task<Guid?> GetUserIdAsync()
        {
            var authState = await AuthProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            return Guid.TryParse(user.FindFirst("sub")?.Value, out var userId) ? userId : (Guid?)null;
        }

        protected async Task DeleteCartItem_Click(Guid CartItemId)
        {
            var cartItemDto = await _shoppingCartService.DeleteItem(CartItemId);

            RemoveCartItem(CartItemId);
            CartChanged();
        }

        protected async Task UpdateQuanlityCartItem_Click(Guid CartItemId, int qty)
        {
            try
            {
                if(qty > 0)
                {
                    var updateItemDto = new CartIemQuanlityUpdateDto
                    {
                        CartItemId = CartItemId,
                        Quanlity = qty

                    };

                    var returnedUpdateItemDto = await this._shoppingCartService.UpdateQuantity(updateItemDto);

                    UpdateItemGrandTotal(returnedUpdateItemDto);
                    UpdateItemQuanlityTotal(returnedUpdateItemDto);
                    CartChanged();
                }
                else
                {
                    var item = this.shoppingCartItems.FirstOrDefault(i => i.ItemId == CartItemId);

                    if (item != null){
                        item.Quanlity = 1;
                        item.Grandtotal = item.Price;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void UpdateItemGrandTotal(CartItemDto cartItemDto)
        {
            var item = GetCartItem(cartItemDto.ItemId);

            if(item != null)
            {
                item.Grandtotal = cartItemDto.Price * cartItemDto.Quanlity;
            }
        }

        private void UpdateItemQuanlityTotal(CartItemDto cartItemDto)
        {
            var item = GetCartItem(cartItemDto.ItemId);

            if (item != null)
            {
                item.Quanlity =  cartItemDto.Quanlity;
            }
        }

        private void CalculateCartSumaryTotals()
        {
            SetGrandTotal();
            SetTotalQuantily();
        }


        private void SetGrandTotal()
        {
            Grandtotal = this.shoppingCartItems.Sum(p =>  p.Grandtotal).ToString("N2") + " đ";
        }

        private void SetTotalQuantily()
        {
            TotalQuantily = this.shoppingCartItems.Sum(p => p.Quanlity);
        }



        private CartItemDto GetCartItem(Guid CartItemId)
        {
            return shoppingCartItems.FirstOrDefault(i => i.ItemId == CartItemId);

        }

        private void RemoveCartItem(Guid CartItemId)
        {
            var cartItemDto = GetCartItem(CartItemId);
            shoppingCartItems.Remove(cartItemDto);
            StateHasChanged(); // Ensure UI is updated
        }

        private void CartChanged()
        {
            CalculateCartSumaryTotals();
            _shoppingCartService.RaiseEventOnShoppingCartChanged(TotalQuantily);
        }


    }
}
