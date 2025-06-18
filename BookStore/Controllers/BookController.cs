using BookStore.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {

        private readonly StoreDbContext _context;

        public BookController(StoreDbContext context)
        {
            _context=context;
        }
        public IActionResult Index()
        {
            var books = _context.Books.ToList();
            return View(books);
        }




    }
}
