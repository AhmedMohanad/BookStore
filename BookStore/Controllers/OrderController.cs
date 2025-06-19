using BookStore.Data;
using BookStore.Models;
using BookStore.Validatore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly StoreDbContext _context;


        public OrderController(StoreDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _context.Orders
                .ToListAsync();

            return Ok(orders);
        }


        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(int id)
        {
            var order = await _context.Orders
                .FindAsync(id);

            if (order == null)
                return NotFound("No order found.");

            return Ok(order);
        }


        [HttpPut]
        public async Task<IActionResult> Edit(int id, [FromBody] Order order)
        {
            if (id != order.Id)
                return BadRequest("Order ID mismatch.");
            if (ModelState.IsValid)
            {
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                return RedirectToAction("index");
            }
            return BadRequest("book not edited.");

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            var order = await _context.Orders
               .FindAsync(id);

            if (order == null)
                return NotFound("No order found.");
             _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return Ok("order deleted successfully.");

        }


        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            if(!ModelState.IsValid )
            {

                return BadRequest("invalid order.");

            }

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();  

            return Ok(order);
        }
    }
}
