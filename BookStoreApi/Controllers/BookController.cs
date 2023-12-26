using AutoMapper;
using BookStoreApi.Models;
using BookStoreApi.Repositories.Interfaces;
using BookStoreView.Models.Dtos;
using BookStoreView.Models.Dtos.DtoProductPortfolio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        public BookController(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
           
        }

        // GET: api/Books
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllBook()
        {
            try
            {
                var books = _bookRepository.GetBookWithOrderList();
                var BookDto = _mapper.Map<List<BookDto>>(books);
                return Ok(BookDto);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/Books/5
        [HttpGet("{bookId:Guid}", Name = "GetBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetBook(Guid bookId)
        {

            if (!_bookRepository.BookExists(bookId))
            {
                return NotFound();
            }
            var BookDto = _mapper.Map<BookDto>(_bookRepository.GetBookWithIdGuid(bookId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(BookDto);
        }

 
        // PUT: api/Books/5
        [HttpPut("{bookId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateBook(Guid bookId, [FromBody] BookDto bookDto)
        {
            if (bookDto == null)
            {
                return BadRequest(ModelState);
            }


            if (!_bookRepository.BookExists(bookId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = _mapper.Map<Book>(bookDto);

            if (!_bookRepository.UpdateBook(book))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {book.Title}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

       
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateBook([FromBody] BookDto bookDto)
        {
            if (bookDto == null)
            {
                return BadRequest("The book cannot be null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_bookRepository.BookNameExists(bookDto.Title))
            {
                return Conflict("A book with the same title already exists.");
            }

            var book = _mapper.Map<Book>(bookDto);

            if (!string.IsNullOrEmpty(bookDto.PathImage))
            {

                book.PathImage = bookDto.PathImage;
            }

            if (!_bookRepository.CreateBook(book))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while saving the book {book.Title}");
            }

            return CreatedAtRoute("GetBook", new { bookId = book.BookId }, book);
        }

     
        // DELETE: api/Books/5
        [HttpDelete("{bookId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteBook(Guid bookId)
        {
            if (!_bookRepository.BookExists(bookId))
            {
                return NotFound();
            }

            var book = _bookRepository.GetBook(bookId);

            if (!_bookRepository.DeleteBook(book))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {book.Title}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
