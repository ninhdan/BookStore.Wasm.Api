using BookStoreView.Models.Dtos;
using BookStoreView.Models.Dtos.DtoProductPortfolio;

namespace BookStoreBlazorWasm.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetBooks();
        public string GetErrorMessage();
        public string GetSuccessMessage();
        Task<BookDto> GetBook(Guid bookId);
        Task<bool> CreateBook(BookDto bookDto);
        Task<bool> UpdateBook(BookDto bookDto);
        Task<bool> DeleteBook(Guid bookId);
       
    }
}
