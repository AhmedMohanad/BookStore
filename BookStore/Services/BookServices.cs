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

        public Book GetBookByTitle(string title)
        {
            var book = _context.Books.FirstOrDefault(x => x.Title == title);
            return book;


        }

        public Book GetBookByAuthor(string author)
        {
            var book = _context.Books.FirstOrDefault(x => x.Author == author);
            return book;


        }
        public Book GetBookByGenre(string genre)
        {
            var book = _context.Books.FirstOrDefault(x => x.Genre == genre);
            return book;


        }
    }
}
