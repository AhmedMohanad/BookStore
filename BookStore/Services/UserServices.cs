using BookStore.Data;
using BookStore.Models;


namespace BookStore.Services
{
    public class UserServices
    {

        private readonly StoreDbContext _context;

        public UserServices(StoreDbContext context) 
        {
            _context = context;
        }

        public User GetUserById(int id)
        {

            var user = _context.Users.SingleOrDefault(x => x.Id == id);
            return user;
        }

    }
}
