using BookStore.Data;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services
{
    public class BookServices
    {

        private readonly StoreDbContext _context;

        public BookServices(StoreDbContext context) 
        {
            _context = context;
        }

        public Book GetBookById(int id)
        {
            var book =  _context.Books.FirstOrDefault(x => x.Id == id);
            return book;

            
        }
    }
}
