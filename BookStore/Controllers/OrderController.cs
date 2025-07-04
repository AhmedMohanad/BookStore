using BookStore.Data;
using BookStore.JWT;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("api/order")] 
    public class OrdersController : ControllerBase  
    {
        private readonly StoreDbContext _context;
        private readonly JWTServices _jwtServices;

        public OrdersController(StoreDbContext context,JWTServices jWTServices)
        {
            _jwtServices = jWTServices;
            _context = context;
           
           

        }

        // Over writed cuz we are using it an another controller :)
        public OrdersController(StoreDbContext context)
        {
           
            _context = context;

        }

        // GET api/order
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var userId = _jwtServices.GetUserIdFromToken(HttpContext);
            if (userId == null) return Unauthorized();
            var orders = await _context.Orders.ToListAsync();
                return Ok(orders);
           
        }

        // GET api/order/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var userId = _jwtServices.GetUserIdFromToken(HttpContext);
            if (userId == null) return Unauthorized();
            var order = await _context.Orders.FindAsync(id);
                return order == null ? NotFound("Order not found") : Ok(order);
           
        }

        // POST api/order
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            var userId = _jwtServices.GetUserIdFromToken(HttpContext);
            if (userId == null) return Unauthorized();
            if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
           
        }

        // PUT api/order/{id}
        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
        {
            var userId = _jwtServices.GetUserIdFromToken(HttpContext);
            if (userId == null) return Unauthorized();
            if (id != order.Id)
                    return BadRequest("ID mismatch");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _context.Entry(order).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
           

        }

        // DELETE api/order/{id}
        [HttpDelete("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrder(int id)
        {

            var userId = _jwtServices.GetUserIdFromToken(HttpContext);
            if (userId == null) return Unauthorized();
            var order = await _context.Orders.FindAsync(id);
                if (order == null)
                    return NotFound();

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                return NoContent();
           
        }
    }
}