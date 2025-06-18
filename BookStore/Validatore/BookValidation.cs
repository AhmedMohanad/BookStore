using BookStore.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Validatore
{
    public class BookValidation
    {

        private readonly StoreDbContext _db;
        public BookValidation(StoreDbContext db)
        {
            _db = db;
        }

        public async Task<bool> isExist(int id)
        {
            return ( await _db.Books.FirstOrDefaultAsync(b => b.Id == id) == null);
        }
    }
}
