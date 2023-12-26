using BookStoreBlazorWasm.Services;
using BookStoreBlazorWasm.Services.Interfaces;
using BookStoreView.Models.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreBlazorWasm.Pages.SellingBooks
{
    public class SellingBooksBase :ComponentBase
    {
        [Inject] private IBookService _bookService { get; set; }

        [Inject] private IShoppingCartService _shoppingCartService { get; set; }

        [Inject] protected NavigationManager navigationManager { get; set; }

        [Inject] AuthenticationStateProvider StateProvider { get; set; }

        protected IEnumerable<BookDto> _books;

        protected IEnumerable<BookDto> _bestselling;

        [Inject] private ISubCategoryService subCategoryService { get; set; }

        protected IEnumerable<SubCategoryDto> _subCategories = new List<SubCategoryDto>();

        protected IEnumerable<SubCategoryDto> RandomsubCategoryDtos = new List<SubCategoryDto>();



        protected override async Task OnInitializedAsync()
        {

            try
            {
                await base.OnInitializedAsync();

                var userId = await GetUserIdAsync();

                await LoadingBooks();
                await LoadingBestSellingBooks();
                await LoadingCartChange();
                await RadomSubCategory();
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        private async Task<Guid?> GetUserIdAsync()
        {
            var authState = await StateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            return Guid.TryParse(user.FindFirst("sub")?.Value, out var userId) ? userId : (Guid?)null;
        }

        private async Task LoadingBooks()
        {
            _books = await _bookService.GetBooks();
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


        protected void NavigateToDetailPage(Guid bookId)
        {
            navigationManager.NavigateTo($"/bookdetails/{bookId}");
        }

        private async Task LoadingBestSellingBooks()
        {
            var allBooks = _books = await _bookService.GetBooks() ?? Enumerable.Empty<BookDto>();
            _bestselling = allBooks.OrderBy(x => Guid.NewGuid()).Take(5).ToList();
        }


        private async Task RadomSubCategory()
        {
                _subCategories = await subCategoryService.GetAllSubCategory();
                Random rnd = new Random();
                RandomsubCategoryDtos = _subCategories.OrderBy(x => rnd.Next()).Take(4).ToList();
        }


        protected void NavigateToBooks(Guid subCategoryId)
        {
            Console.WriteLine($"Navigating to books with SubCategoryId: {subCategoryId}");
            navigationManager.NavigateTo($"/books/{subCategoryId}");
        }

    }
}
