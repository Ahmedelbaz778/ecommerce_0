using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Models;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderitemsController : ControllerBase
    {
        private readonly Econtext _context;

        public OrderitemsController(Econtext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orderitems>>> GetOrderItems()
        {
            return await _context.Orderitems
                .Include(oi => oi.orders)
                .Include(oi => oi.product)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Orderitems>> GetOrderItem(int id)
        {
            var orderItem = await _context.Orderitems
                .Include(oi => oi.orders)
                .Include(oi => oi.product)
                .FirstOrDefaultAsync(oi => oi.Id == id);

            if (orderItem == null)
                return NotFound();

            return orderItem;
        }

        [HttpPost]
        public async Task<ActionResult<Orderitems>> CreateOrderItem(Orderitems orderItem)
        {
            _context.Orderitems.Add(orderItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderItem), new { id = orderItem.Id }, orderItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItem(int id, Orderitems orderItem)
        {
            if (id != orderItem.Id)
                return BadRequest();

            _context.Entry(orderItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Orderitems.Any(oi => oi.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var orderItem = await _context.Orderitems.FindAsync(id);
            if (orderItem == null)
                return NotFound();

            _context.Orderitems.Remove(orderItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
