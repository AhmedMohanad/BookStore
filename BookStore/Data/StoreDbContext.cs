using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data
{
    public class StoreDbContext: DbContext
    {
        public DbSet<Book> Books { get; set; }
    }
}
