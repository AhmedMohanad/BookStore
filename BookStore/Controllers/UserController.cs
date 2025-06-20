using BookStore.Data;
using BookStore.DTOs;
using BookStore.Models;
using BookStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{

    [ApiController]
    [Route("api/user")] // Pluralized resource name
    public class UserController : Controller
    {
       
        private readonly StoreDbContext _context;
        private readonly IConfiguration _config;
        private CartServices _cs;

        public UserController(StoreDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
          
            
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.OrderBy(u => u.Name).ToListAsync();

            if (users.Count == 0 ) 
            return Ok("No users found.");

            return Ok(users);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromBody]RegisterDto dto)
        {
            _cs = new CartServices(_context);

            if (_context.Users.Any(u => u.Email == dto.Email))
                return BadRequest("Email already used try another one.");


           int cartId =  _cs.CreateCart();
            var user = new User
            {
                Name = dto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                CartId = cartId,
                Email = dto.Email,
                
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok($"Welcome in our site {user.Name} ");
        }



    }
}
