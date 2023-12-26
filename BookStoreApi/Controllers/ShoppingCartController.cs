using BookStoreApi.Extensions;
using BookStoreApi.Repositories.Interfaces;
using BookStoreView.Models.Dtos.ShoppingCart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IBookRepository bookRepository;

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IBookRepository bookRepository) {
            this.shoppingCartRepository = shoppingCartRepository;
            this.bookRepository = bookRepository;
        }

        [HttpGet]
        [Route("{customerId}/GetItems")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetItems (Guid customerId)
        {
            try
            {
                var cartItems = await this.shoppingCartRepository.GetItems(customerId);

                if(cartItems == null)
                {
                    return NoContent();
                }
                var books =  this.bookRepository.GetAllBook();

                if(books == null)
                {
                    throw new Exception("No books exist in the system");
                }
                var cartItemDto = cartItems.ConvertToDto(books);
                return Ok(cartItemDto);

            }catch(Exception ex)
            {

                return StatusCode(500, $"Internal Server Error: {ex.Message}");


            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CartItemDto>> GetItem(Guid id)
        {

            try{ 
            
                var cartItem = await this.shoppingCartRepository.GetItem(id);

                if(cartItem == null)
                {
                    return NotFound();
                }
                var book = bookRepository.GetBook(cartItem.BookId);

                if(book == null)
                {
                    return NotFound();
                }

                var cartItemDto = cartItem.ConvertToDto(book);
                return Ok(cartItemDto);

            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }

        [HttpPost]
        public async Task<ActionResult<CartItemDto>> PostItem([FromBody] CartItemToAddDto cartItemToAddDto, [FromQuery] Guid userId)
        {
            try
            {
                var newCartItem = await this.shoppingCartRepository.AddItem(cartItemToAddDto, userId);
                if(newCartItem == null)
                {
                    return NotFound();
                }

                var book = bookRepository.GetBook(newCartItem.BookId);

                if(book == null)
                {
                    throw new Exception($"Something went wrong when attemting to retrieve book (bookId : ({cartItemToAddDto.BookId})) ");

                }


                var newCartItemDto = newCartItem.ConvertToDto(book);

                return CreatedAtAction(nameof(GetItem), new {id = newCartItemDto.ItemId}, newCartItemDto);

            }catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CartItemDto>> DeleteItem(Guid id)
        {
            try
            {
                var cartItem = await this.shoppingCartRepository.DeleteItem(id);
                if(cartItem == null)
                {
                    return NotFound();
                }

                var book = this.bookRepository.GetBook(cartItem.BookId);

                if(book == null)
                {
                    return NotFound();
                }

                var cartItemDto = cartItem.ConvertToDto(book);

                return Ok(cartItemDto);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<CartItemDto>> UpdateQuanlity(Guid id , CartIemQuanlityUpdateDto cartIemQuanlityUpdateDto)
        {
            try
            {
                var carttItem = await this.shoppingCartRepository.UpdateQuanlity(id, cartIemQuanlityUpdateDto);

                if(carttItem == null)
                { 
                    return NotFound(); 
                }

                var book = bookRepository.GetBook(carttItem.BookId);
                
                var cartItemDto = carttItem.ConvertToDto(book);

                return Ok(cartItemDto);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        
    }
}
