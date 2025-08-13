using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Models;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscribersController : ControllerBase
    {
        private readonly Econtext _context;

        public SubscribersController(Econtext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subscribers>>> GetSubscribers()
        {
            return await _context.Subscribers.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Subscribers>> GetSubscriber(int id)
        {
            var subscriber = await _context.Subscribers.FindAsync(id);

            if (subscriber == null)
                return NotFound();

            return subscriber;
        }

        [HttpPost]
        public async Task<ActionResult<Subscribers>> CreateSubscriber(Subscribers subscriber)
        {
            _context.Subscribers.Add(subscriber);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSubscriber), new { id = subscriber.Id }, subscriber);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscriber(int id)
        {
            var subscriber = await _context.Subscribers.FindAsync(id);
            if (subscriber == null)
                return NotFound();

            _context.Subscribers.Remove(subscriber);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
