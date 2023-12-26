
using BookStoreApi.Models;
using BookStoreApi.Repositories.Interfaces;
using BookStoreView.Models.Dtos;
using BookStoreView.Models.Dtos.ShoppingCart;
using Microsoft.EntityFrameworkCore;
using UnidecodeSharpCore;

namespace BookStoreApi.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly DbWater7Context _context;

        public BookRepository(DbWater7Context context)
        {
            _context = context;
        }

        public bool BookExists(Guid BookId)
        {
            return _context.Books.Any(c => c.BookId == BookId);
        }

        public bool BookNameExists(string Titile)
        {
            return _context.Books.Any(c => c.Title.ToLower().Trim() == Titile.ToLower().Trim());
        }

        public bool CreateBook(Book Book)
        {
            _context.Add(Book);
            return Save();
        }

        public bool DeleteBook(Book Book)
        {
            _context.Remove(Book);
            return Save();
        }

        public ICollection<Book> GetAllBook()
        {
           return  _context.Books.ToList();
        }

        public Book GetBook(Guid BookId)
        {
            return _context.Books.Where(c => c.BookId == BookId).FirstOrDefault();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateBook(Book Book)
        {
            _context.Update(Book);
            return Save();
        }

        public List<BookDto> GetBookWithOrderList()
        {
            var bookDtos = _context.Books
           .Select(book => new BookDto
           {
               BookId = book.BookId,
               Title = book.Title,
               PathImage = book.PathImage,
               Description = book.Description,
               Price = book.Price,
               Author = book.Author,
               SubcategoryId = book.SubcategoryId,
               SubcategoryName = book.Subcategory.SubcategoryName,
               SupplierId = book.SupplierId,
               SupplierName = book.Supplier.SupplierName,
               LanguageId = book.LanguageId,
               LanguageName = book.Language.LanguageName,
               LayoutId = book.LayoutId,
               LayoutName = book.Layout.LayoutName,
               PriceRangeId = book.PriceRangeId,
               PriceRangeName = book.PriceRange.RangeName,
               Publisher = book.Publisher,
               PublisherYear = book.PublisherYear,
               QuanlityPage = book.QuanlityPage,
               Weight = book.Weight,
               Translator = book.Translator,
               Size = book.Size,

           }).ToList();

            return bookDtos;
        }

        public BookDto GetBookWithIdGuid(Guid bookId)
        {
            var book = _context.Books
                .Where(b => b.BookId == bookId)
                .Select(b => new BookDto
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    Author = b.Author,
                    Publisher = b.Publisher,
                    PublisherYear = b.PublisherYear,
                    Size = b.Size,
                    Translator = b.Translator,
                    Weight = b.Weight,
                    QuanlityPage = b.QuanlityPage,
                    Description = b.Description,
                    SupplierId = b.SupplierId,
                    SupplierName = b.Supplier.SupplierName,
                    LanguageId = b.LanguageId,
                    LanguageName = b.Language.LanguageName,
                    SubcategoryId = b.SubcategoryId,
                    SubcategoryName = b.Subcategory.SubcategoryName,
                    LayoutId = b.LayoutId,
                    LayoutName = b.Layout.LayoutName,
                    PriceRangeId = b.PriceRangeId,
                    PriceRangeName = b.PriceRange.RangeName,
                    Price = b.Price,
                    PathImage = b.PathImage,
                   
                })
                .FirstOrDefault();

            return book;
        }
    }
}
