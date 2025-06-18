using BookStore.Data;
using BookStore.Models;
using BookStore.Validatore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace BookStore.Controllers
{

    [ApiController]
    [Route("api/book")]
    public class BookController : Controller
    {

        private readonly StoreDbContext _context;
        private readonly BookValidation _vlaidation;

        public BookController(StoreDbContext context)
        {
            _vlaidation = new BookValidation(context);
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var books =  await _context.Books
               .OrderBy(b => b.Title)
               .ToListAsync();
            return Ok(books);
        }

        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            
            if (ModelState.IsValid && ! book.Title.IsNullOrEmpty())
            {
                if (book.Stock > 0) 
                    book.IsAvailable = true;
                else
                    book.IsAvailable = false;

                    await _context.Books.AddAsync(book);
               await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }

            return BadRequest("error! check title should not be null.");
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (await _vlaidation.isExist(id))
            {
                var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
                return Ok(book);
            }
            return NotFound("book not exist.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromBody] Book book)
        {

            if (ModelState.IsValid)
            {
                _context.Books.Update(book);
                await _context.SaveChangesAsync();
                return RedirectToAction("index");
            }
            return BadRequest("book not edited.");
        }

       

        public async Task<IActionResult> Delete(int id)
        {
            if (await _vlaidation.isExist(id))
            {
                var item = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
                return Ok(item);
            }
            return BadRequest("book not exist.");
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Books.FindAsync(id);
            if (item != null)
            {
                _context.Books.Remove(item);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction("Index");
        }








    }
}
