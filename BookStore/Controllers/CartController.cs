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
        public CartController(StoreDbContext context,JWTServices jWTServices)
        {
            _rdersController = new OrdersController(context);
            _cs = new CartServices(context);
            _context = context;
            _jwtServices = jWTServices;


        }


        

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = _jwtServices.GetUserIdFromToken(HttpContext);
            if (userId == null) return Unauthorized();

            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.Id == id);
                if (cart == null)
                    return NotFound("no cart found.");
                var items = cart.Books?.ToList();

                return Ok(items);
            
           
        }

        [HttpPost("{cartId}/{itemId}")]
        public async Task<IActionResult> AddItem(int cartId,int itemId)
        {
            var userId = _jwtServices.GetUserIdFromToken(HttpContext);
            if (userId == null) return Unauthorized();

            var cart = await _context.Carts
                .Include(c => c.Books)
               .FirstOrDefaultAsync(c => c.Id == cartId);
                if (cart == null) return BadRequest("this cart not exist");
                var item = await _context.Books.FirstOrDefaultAsync(i => i.Id == itemId);
                if (item == null) return BadRequest("this item not exist.");

                cart.Books?.Add(itemId);
                await _context.SaveChangesAsync();
                return Ok(cart);
       


        }



        [HttpDelete("{cartId}/{itemId}")]
        public async Task<IActionResult> Delete(int cartId, int itemId)
        {
            var userId = _jwtServices.GetUserIdFromToken(HttpContext);
            if (userId == null) return Unauthorized();
            var cart = await _context.Carts.FindAsync(cartId);
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


        [HttpPost("{cartId}/buy")]
        public async Task<IActionResult> BuyCart(int cartId)
        {
            var userIdent = _jwtServices.GetUserIdFromToken(HttpContext);
            if (userIdent == null) return Unauthorized();
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.Id == cartId);
                if (cart == null) { return NotFound("cart not exist"); }

                var books = cart.Books.ToList();
                double total = _cs.CalculateTotale(books);

                int userId = (int)cart.UserId;
                DateTime time = DateTime.Now;
                Order order = new Order()
                {
                    TotalAmount = total,
                    OrderDate = time,
                    UserId = userId
                };
                _rdersController.CreateOrder(order);




                return Ok(order);
        




}



    }
}
