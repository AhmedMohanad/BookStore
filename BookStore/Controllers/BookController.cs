using BookStore.Data;
using BookStore.JWT;
using BookStore.Models;
using BookStore.Validatore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("api/book")] // Pluralized resource name
    public class BooksController : ControllerBase // Changed to ControllerBase for API
    {

      
        private readonly StoreDbContext _context;
        private readonly BookValidation _validation;
        private readonly JWTServices _jwtServices;

        public BooksController(StoreDbContext context, JWTServices jWTServices)
        {
           
            _context = context;
            _validation = new BookValidation(context);
            _jwtServices = jWTServices;
           
        }

        // GET api/books
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _context.Books
                .OrderBy(b => b.Title)
                .ToListAsync();
            return Ok(books);
        }

        // GET api/books/{id}
       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            if (!await _validation.isExist(id))
                return NotFound("Book not found");

            var book = await _context.Books.FindAsync(id);
            return Ok(book);
        }

        // POST api/books
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] Book book)
        {


            var userId = _jwtServices.GetUserIdFromToken(HttpContext);
            if (userId == null) return Unauthorized();

            if (!ModelState.IsValid || string.IsNullOrEmpty(book.Title))
                    return BadRequest("Invalid book data");

                book.IsAvailable = book.Stock > 0;

                await _context.Books.AddAsync(book);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
          
        }

        // PUT api/books/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book book)
        {

            var userId = _jwtServices.GetUserIdFromToken(HttpContext);
            if (userId == null) return Unauthorized();

            if (id != book.Id)
                    return BadRequest("ID mismatch");

                if (!ModelState.IsValid)
                    return BadRequest("Invalid book data");

                if (!await _validation.isExist(id))
                    return NotFound("Book not found");

                _context.Entry(book).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
        

        // DELETE api/books/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {


            var userId = _jwtServices.GetUserIdFromToken(HttpContext);
            if (userId == null) return Unauthorized();
            var book = await _context.Books.FindAsync(id);
                if (book == null)
                    return NotFound();

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();


                return NoContent();
           
        }
    }
}