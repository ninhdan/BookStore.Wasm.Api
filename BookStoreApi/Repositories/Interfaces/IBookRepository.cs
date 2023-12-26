using BookStoreApi.Models;
using BookStoreView.Models.Dtos;

namespace BookStoreApi.Repositories.Interfaces
{
    public interface IBookRepository
    {
        ICollection<Book> GetAllBook();
        BookDto GetBookWithIdGuid(Guid bookId);
        Book GetBook(Guid BookId);
        bool BookExists(Guid BookId);
        bool BookNameExists(string BookName);
        bool CreateBook(Book Book);
        bool UpdateBook(Book Book);
        bool DeleteBook(Book Book);
        List<BookDto> GetBookWithOrderList();
        bool Save();

    }
}
