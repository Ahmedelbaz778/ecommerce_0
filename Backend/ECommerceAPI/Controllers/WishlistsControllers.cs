using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Models;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistsController : ControllerBase
    {
        private readonly Econtext _context;

        public WishlistsController(Econtext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Wishlists>>> GetWishlists()
        {
            return await _context.Wishlists
                .Include(w => w.users)
                .Include(w => w.product)
                .ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Wishlists>> GetWishlist(int id)
        {
            var wishlist = await _context.Wishlists.FindAsync(id);

            if (wishlist == null)
                return NotFound();

            return wishlist;
        }

        [HttpPost]
        public async Task<ActionResult<Wishlists>> CreateWishlist(Wishlists wishlist)
        {
            _context.Wishlists.Add(wishlist);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWishlist), new { id = wishlist.Id }, wishlist);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWishlist(int id, Wishlists wishlist)
        {
            if (id != wishlist.Id)
                return BadRequest();

            _context.Entry(wishlist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Wishlists.Any(w => w.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWishlist(int id)
        {
            var wishlist = await _context.Wishlists.FindAsync(id);
            if (wishlist == null)
                return NotFound();

            _context.Wishlists.Remove(wishlist);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
