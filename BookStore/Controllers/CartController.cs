using BookStore.Data;
using BookStore.JWT;
using BookStore.Models;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{

    [ApiController]
    [Route("api/cart")]
    public class CartController : Controller
    {
        private OrdersController _rdersController;
        private CartServices _cs;
        private readonly StoreDbContext _context;
        private readonly JWTServices _jwtServices;
        private readonly UserServices _userServices;
        public CartController(StoreDbContext context,JWTServices jWTServices)
        {
            _rdersController = new OrdersController(context);
            _cs = new CartServices(context);
            _context = context;
            _jwtServices = jWTServices;
            _userServices = new UserServices(context);


        }



        // Get api/cart/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = _jwtServices.GetUserIdFromToken(HttpContext);
            if (userId == null) return Unauthorized();

            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.Id == id);
                if (cart == null)
                    return NotFound("no cart found.");
                

                return Ok(cart);
            
           
        }

        // Post api/cart/{id}
        [HttpPost("{itemId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(int itemId)
        {
          
            var userId = _jwtServices.GetUserIdFromToken(HttpContext);
            if (userId == null) return Unauthorized("Please log in first");

           
            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null) return NotFound("User not found");

           
            var cart = await _context.Carts.FindAsync(user.CartId);
            if (cart == null) return BadRequest("Cart not found");

           
            var bookExists = await _context.Books.AnyAsync(b => b.Id == itemId);
            if (!bookExists) return BadRequest("Book not found");

          
            cart.Books ??= new List<int>();

          
            if (cart.Books.Contains(itemId))
                return BadRequest("Book already in cart");

          
            cart.Books.Add(itemId);

          
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    Message = "Book added to cart",
                    CartId = cart.Id,
                    BookId = itemId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Failed to update cart: " + ex.Message);
            }
        }


        // Delete api/cart/{id}
        [HttpDelete("{itemId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete( int itemId)
        {
            var userId = _jwtServices.GetUserIdFromToken(HttpContext);
            if (userId == null) return Unauthorized();


            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null) return NotFound("User not found");


            var cartCheck = await _context.Carts.FindAsync(user.CartId);
            if (cartCheck == null) return BadRequest("Cart not found");


            var cart = await _context.Carts.FindAsync(cartCheck.Id);
                if (cart == null)
                {
                    return NotFound(" cart is empty.");
                }

                if (cart.Books == null || !cart.Books.Contains(itemId))
                {
                    return NotFound("Item not found in cart");
                }

                cart.Books.Remove(itemId);
                await _context.SaveChangesAsync();

                return NoContent();
           
        }

        // Post api/cart/buy/{id}
        // will empty the cart and create an order :)
        [HttpPost("buy/{cartId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyCart(int cartId)
        {
            var userIdent = _jwtServices.GetUserIdFromToken(HttpContext);
            if (userIdent == null) return Unauthorized();
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.Id == cartId);
                if (cart == null) { return NotFound("cart not exist"); }

                var books = cart.Books.ToList();
                double total = _cs.CalculateTotale(books);

             
                DateTime time = DateTime.Now;
                Order order = new Order()
                {
                    TotalAmount = total,
                    OrderDate = time,
                    UserId = cart.Id
                };
                _rdersController.CreateOrder(order);




                return Ok(order);





        } 
        
        
        
        // Get api/cart/getcartbyid/{id}
        [HttpGet("getcartbyid/{id}")]
        public async Task<IActionResult> GetCartById(int id)
        {
            var userIdent = _jwtServices.GetUserIdFromToken(HttpContext);
            if (userIdent == null) return Unauthorized();

            var cart =await _context.Carts.FirstOrDefaultAsync(c => c.Id == id);
            return Ok(cart);
        }




        
        // Put api/cart/edit/{id}
        [HttpPut("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCart(int id,Cart newCart)
        {
            var userIdent = _jwtServices.GetUserIdFromToken(HttpContext);
            if (userIdent == null) return Unauthorized();

            if(id != newCart.Id)
            {
                return BadRequest();
            }
            _context.Entry(newCart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (await _context.Carts.FindAsync(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }
            return NoContent();
        }



    }
}
