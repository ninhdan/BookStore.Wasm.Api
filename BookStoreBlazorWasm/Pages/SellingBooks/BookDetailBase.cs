using Blazored.Modal;
using Blazored.Modal.Services;
using BookStoreBlazorWasm.Services;
using BookStoreBlazorWasm.Services.Interfaces;
using BookStoreView.Models.Dtos;
using BookStoreView.Models.Dtos.ShoppingCart;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreBlazorWasm.Pages.SellingBooks
{
    public class BookDetailBase : ComponentBase
    {

        [Inject] IBookService _bookService { get; set; }

        [Inject] IShoppingCartService _shoppingCartService { get; set; }

        [Inject] NavigationManager NavigationManager { get; set; }

        [Inject] AuthenticationStateProvider AuthProvider { get; set; }
        [CascadingParameter] BlazoredModalInstance BlazoredModalInstance { get; set; }

        [Inject] IModalService Modal { get; set; }

        [Parameter] public Guid BookId { get; set; }

        protected BookDto bookDto = new BookDto();

        protected IEnumerable<BookDto> _books;

        protected int Value  {get; set;} = 1;

        protected override async Task OnParametersSetAsync()
        {
            await base.OnInitializedAsync();

            var userId = await GetUserIdAsync();

            await LoadBookDetails();
            await LoadingBooks();
            await LoadingCartChange();

        }

     
        private async Task LoadBookDetails()
        {
            bookDto = await _bookService.GetBook(BookId);

            if (bookDto == null)
            {
                NavigationManager.NavigateTo("/notfound");
            }
        }

        private async Task LoadingBooks()
        {
            var allBooks = _books = await _bookService.GetBooks() ?? Enumerable.Empty<BookDto>();
            _books = allBooks.OrderBy(x => Guid.NewGuid()).Take(5).ToList();
        }

        protected void NavigateToDetailPage(Guid bookId)
        {
            NavigationManager.NavigateTo($"/bookdetails/{bookId}");
        }



        private async Task LoadingCartChange()
        {
            var userId = await GetUserIdAsync();
            if (userId.HasValue)
            {
                var shoppingCartItems = await _shoppingCartService.GetItems(userId.Value);
                var totalQty = shoppingCartItems.Sum(i => i.Quanlity);
                _shoppingCartService.RaiseEventOnShoppingCartChanged(totalQty);
            }
        }

        private async Task<Guid?> GetUserIdAsync()
        {
            var authState = await AuthProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            return Guid.TryParse(user.FindFirst("sub")?.Value, out var userId) ? userId : (Guid?) null;
        }

        protected async Task AddToCart_Click(CartItemToAddDto cartItemToAdd)
        {
            try
            {
                var userId = await GetUserIdAsync();
                if(userId.HasValue)
                {
                    var cartItemDto = await _shoppingCartService.AddItem(cartItemToAdd, userId.Value);
                    NavigationManager.NavigateTo("/ShoppingCart");
                }
                else
                {
                    var returnUrl = NavigationManager.Uri;

                    // Kiểm tra xem URL có hợp lệ không
                    if (Uri.IsWellFormedUriString(returnUrl, UriKind.RelativeOrAbsolute))
                    {
                        // Kiểm tra xem URL có thuộc về ứng dụng của bạn không
                        var appUri = new Uri(NavigationManager.BaseUri);
                        var returnUri = new Uri(returnUrl, UriKind.RelativeOrAbsolute);

                        if (!returnUri.IsAbsoluteUri || returnUri.Authority == appUri.Authority)
                        {
                            // URL hợp lệ và thuộc về ứng dụng của bạn
                            NavigationManager.NavigateTo($"/login?returnUrl={Uri.EscapeDataString(returnUrl)}", true);
                        }
                       
                    }
                   

                }

            }
            catch (Exception)
            {

                throw;
            }
        }






    }
}
