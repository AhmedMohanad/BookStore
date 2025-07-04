using BookStore.Data;
using BookStore.DTOs;
using BookStore.JWT;
using BookStore.Models;
using BookStore.Services;
using BookStore.Validatore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStore.Controllers
{

    [ApiController]
    [Route("api/user")] 
    public class UserController : Controller
    {
       
        private readonly StoreDbContext _context;
        private CartServices _cs;
        private readonly JWTServices _jWTService;
       

        public UserController(StoreDbContext context,JWTServices jWT)
        {
            _context = context;
            _jWTService = jWT;
        

        }


        // Get api/user
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.OrderBy(u => u.Name).ToListAsync();

            if (users.Count == 0 ) 
            return Ok("No users found.");

            return Ok(users);
        }

        // Post api/user/register
        //this method will sinup and login in by calling login action :)
        [HttpPost("register")]
       
        public async Task<IActionResult> Register([FromBody]RegisterDto dto)
        {
            if (!EmailValidation.ValidateEmail(dto.Email))
                return BadRequest("invalid email format");
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

            this.Login(new LoginDto { Email = dto.Email,Password = dto.Password});

            return Ok($"Welcome in our site {user.Name} ");
        }



        // Post api/user/login
        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                return Unauthorized("Invalid credentials.");

            var jwt = _jWTService.Generate(user.Id);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.Now.AddDays(7)
            });

            return Ok("success");
        }

        // Post api/user/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
            return Ok("Bye By");
        }






        // ** this method are used to  "" authenticate "" the requests in any controller **
        public bool User()
        {

            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = _jWTService.Verify(jwt);
                int userId = int.Parse(token.Issuer);
                var user = _context.Users.FirstOrDefault(i => i.Id == userId);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

