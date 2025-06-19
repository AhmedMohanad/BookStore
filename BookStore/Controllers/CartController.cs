using BookStore.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{

    [ApiController]
    [Route("api/cart")]
    public class CartController : Controller
    {

        private readonly StoreDbContext _context;

        public CartController(StoreDbContext context)
        {
            _context = context;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null) 
                return NotFound("no cart found.");
            var items =  cart.Books?.ToList();

            return Ok(items);
        }

        [HttpDelete("{cartId}/{itemId}")]
        public async Task<IActionResult> Delete(int cartId, int itemId)
        {
            var cart = await _context.Carts.FindAsync(cartId);
            if (cart == null)
            {
                return NotFound("no cart is empty.");
            }

            if (cart.Books == null || !cart.Books.Contains(itemId))
            {
                return NotFound("Item not found in cart");
            }

            cart.Books.Remove(itemId);
            await _context.SaveChangesAsync();

            return NoContent();  
        }




    }
}
