using BookStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{

    [ApiController]
    [Route("api/user")] // Pluralized resource name
    public class UserController : Controller
    {

        private readonly StoreDbContext _context;

        public UserController(StoreDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.OrderBy(u => u.Name).ToListAsync();

            if (users.Count == 0 ) 
            return Ok("No users found.");

            return Ok(users);
        }
    }
}
