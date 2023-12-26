using BookStoreBlazorWasm.Services.Interfaces;
using BookStoreView.Models.Dtos.ShoppingCart;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Runtime.Serialization;

namespace BookStoreBlazorWasm.Pages.SellingBooks
{
    public class CheckoutBase : ComponentBase
    {
        [Inject]
        public IJSRuntime JS { get; set; }
        [Inject] AuthenticationStateProvider authenticationState { get; set; }
        protected IEnumerable<CartItemDto> ShoppingCartItems { get; set; }

        protected int TotalQty { get; set; }

        protected string PaymentDescription { get; set; }
        protected decimal PaymentAmount { get; set; }

        [Inject]
        public IShoppingCartService shoppingCartService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await LoaingPaymentPayPal();

            }
            catch (Exception)
            {

                throw;
            }
        }


        private async Task LoaingPaymentPayPal()
        {
            var userId = await GetUserIdAsync();
            if (userId.HasValue)
            {
                ShoppingCartItems = await shoppingCartService.GetItems(userId.Value);


                if (ShoppingCartItems != null)
                {
                    var orderId = ShoppingCartItems.First().OrderId;

                    PaymentAmount = Math.Round(ShoppingCartItems.Sum(p => p.Grandtotal), 2);
                    TotalQty = ShoppingCartItems.Sum(p => p.Quanlity);
                    PaymentDescription = $"O_{userId.Value}_{orderId}";
                }

            }

        }


        private async Task<Guid?> GetUserIdAsync()
        {
            var authState = await authenticationState.GetAuthenticationStateAsync();
            var user = authState.User;

            return Guid.TryParse(user.FindFirst("sub")?.Value, out var userId) ? userId : (Guid?)null;
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if(firstRender)
                {
                    await JS.InvokeVoidAsync("initPayPalButton");
                }    
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
