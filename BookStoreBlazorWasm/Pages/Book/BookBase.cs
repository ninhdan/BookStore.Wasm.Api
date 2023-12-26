using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using BookStoreBlazorWasm.Services.Interfaces;
using BookStoreView.Models.Dtos;
using Microsoft.AspNetCore.Components;


namespace BookStoreBlazorWasm.Pages.Book
{
    public class BookBase:ComponentBase
    {
        [Inject] private IBookService _bookService { get; set; }
        [Inject] private IToastService toastService { get; set; }

        [Inject] protected NavigationManager navigationManage { get; set; }

        [Inject] private IModalService Modal { get; set; } = default!;

        [CascadingParameter] BlazoredModalInstance BlazoredModalInstance { get; set; } = default!;

        public IEnumerable<BookDto> _books;

        public string ErrorMessage { get; set; }

        protected BookDto newBook = new BookDto();

        protected bool IsAnyBookSelected => _books.Count(c => c.IsSelected) > 0;

        protected bool IsSingleBookSelected => _books.Count(c => c.IsSelected) == 1;

        protected List<string> SelectedBookNames => _books?.Where(c => c.IsSelected).Select(c => c.Title).ToList() ?? new List<string>();

        [Parameter] public List<BookDto> SelectedBooks { get; set; } = new List<BookDto>();

        protected bool selectAll;

        protected void SelectAll(bool value)
        {
            selectAll = value;
            foreach (var book in _books)
            {
                book.IsSelected = value;
            }
        }


        protected override async Task OnInitializedAsync()
        {
            await LoadBooks();
        }

        private async Task LoadBooks()
        {
            _books = await _bookService.GetBooks();
        }

        protected void GotoCreateBook()
        {
            navigationManage.NavigateTo("/admin/book/createbook");
        }
        protected void GotoBack()
        {
            navigationManage.NavigateTo("/admin/book");
        }

      
        protected async Task AddNewBook()
        {
            try
            {
                var result = await _bookService.CreateBook(newBook);

                if (result)
                {
 
                    await RefreshUIAfterAddingBook();
                    string successMessage = _bookService.GetSuccessMessage();
                    ShowSuccessMessage(successMessage);
                }
                else
                {
                    
                    string errorMessage = _bookService.GetErrorMessage();
                    ShowErrorMessage(errorMessage);
                }
            }
            catch (Exception ex)
            {
                // Nếu có lỗi xảy ra trong quá trình xử lý, hiển thị thông báo lỗi
                ShowErrorMessage($"An error occurred: {ex.Message}");
            }
        }




        private async Task RefreshUIAfterAddingBook()
        {
            await LoadBooks();
            ResetNewBook();
        }

        private void ResetNewBook()
        {
            newBook = new BookDto();
        }

        protected void ShowSuccessMessage(string message)
        {

            toastService.ShowSuccess(message);
        }

        protected void ShowErrorMessage(string message)
        {
            toastService.ShowError(message);
        }

        protected async Task CloseModal()
        {
            await BlazoredModalInstance.CloseAsync(ModalResult.Cancel(true));
        }

        private async Task RefreshUIAfterDeletingBook(List<BookDto> deletedBooks)
        {

            _books = _books.Except(deletedBooks).ToList();
            string successMessage = _bookService.GetSuccessMessage();
            ShowSuccessMessage(successMessage);
            await LoadBooks();
        }
        protected async Task DeleteSelected()
        {
            var selectedBook = _books?.Where(c => c.IsSelected).ToList();

            if (selectedBook != null && selectedBook.Any())
            {
                SelectedBooks = selectedBook; 
                var parameters = new ModalParameters();
                parameters.Add("SelectedBookNames", SelectedBookNames);

                var modalInstance = Modal.Show<DeleteBook>("Delete Confirmation", parameters);
                var modalResult = await modalInstance.Result;

                if (!modalResult.Cancelled)
                {
                    await Delete();
                    await RefreshUIAfterDeletingBook(selectedBook);
                }
            }
            else
            {
                ShowErrorMessage("No book selected for deletion.");
            }
        }


        protected async Task Cancel()
        {
            if (BlazoredModalInstance != null)
            {
                await BlazoredModalInstance.CancelAsync();
            }
        }


        protected async Task Delete()
        {
            try
            {
                if (_bookService != null)
                {

                    foreach (var book in SelectedBooks)
                    {
                        await _bookService.DeleteBook(book.BookId);
                    }
                    await BlazoredModalInstance.CloseAsync();

                }
                else
                {
                    ShowErrorMessage("BookService is null.");
                }
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine($"An error occurred during deletion: {ex.Message}");

            }

        }

        [Parameter] public BookDto BookToEdit { get; set; } = new BookDto();


        protected async Task ShowEditModal()
        {
            var selectedbook = _books.FirstOrDefault(c => c.IsSelected);
            if (IsSingleBookSelected)
            {
                BookToEdit = selectedbook; // Set the selected category to edit


                var parameters = new ModalParameters();
                parameters.Add("BookToEdit", BookToEdit); // Pass the category to edit to the modal

                var modalInstance = Modal.Show<EditPage>("Edit Book", parameters);
                var modalResult = await modalInstance.Result;

                if (!modalResult.Cancelled)
                {
                    await UpdateBook();

                }
            }
            else
            {
                ShowErrorMessage("Please select a single category for editing.");
            }
        }

        protected async Task UpdateBook()
        {
            try
            {

                var result = await _bookService.UpdateBook(BookToEdit);
                if (result)
                {

                    string successMessage = _bookService.GetSuccessMessage();
                    ShowSuccessMessage(successMessage);
                    await RefreshUIAfterUpdatingBook();

                }
                else
                {
                    string errorMessage = _bookService.GetErrorMessage();
                    ShowErrorMessage(errorMessage);
                }

            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");

            }

        }

        private async Task RefreshUIAfterUpdatingBook()
        {
            await LoadBooks();
            await BlazoredModalInstance.CloseAsync(ModalResult.Cancel(true));
        }





    }
}
