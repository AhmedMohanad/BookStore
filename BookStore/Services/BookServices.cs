using BookStore.Data;
using BookStore.Models;
using Humanizer.Localisation;
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

        public List<Book> GetBooksByTitle(string title)
        {
            return _context.Books
                .Where(b => b.Title.ToLower() == title.ToLower())
                .ToList();
        }

        public List<Book> GetBooksByAuthor(string author)
        {
            return _context.Books
                .Where(b => b.Author.ToLower() == author.ToLower())
                .ToList();
        }

        public List<Book> GetBooksByGenre(string genre)
        {
            return _context.Books
                .Where(b => b.Genre.ToLower() == genre.ToLower())
                .ToList();
        }
    }
}
