using BookStore.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Validatore
{
    public class OrderValidation
    {

        private readonly StoreDbContext _db;
        public OrderValidation(StoreDbContext db)
        {
            _db = db;
        }

        public async Task<bool> isExist(int id)
        {
            return (await _db.Orders.FirstOrDefaultAsync(b => b.Id == id) == null);
        }
    }
}
