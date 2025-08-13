using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Models;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogpostsController : ControllerBase
    {
        private readonly Econtext _context;

        public BlogpostsController(Econtext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Blogposts>>> GetBlogposts()
        {
            return await _context.Blogposts.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Blogposts>> GetBlogpost(int id)
        {
            var blogpost = await _context.Blogposts.FindAsync(id);

            if (blogpost == null)
                return NotFound();

            return blogpost;
        }

        [HttpPost]
        public async Task<ActionResult<Blogposts>> CreateBlogpost(Blogposts blogpost)
        {
            _context.Blogposts.Add(blogpost);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBlogpost), new { id = blogpost.Id }, blogpost);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlogpost(int id, Blogposts blogpost)
        {
            if (id != blogpost.Id)
                return BadRequest();

            _context.Entry(blogpost).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Blogposts.Any(b => b.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogpost(int id)
        {
            var blogpost = await _context.Blogposts.FindAsync(id);
            if (blogpost == null)
                return NotFound();

            _context.Blogposts.Remove(blogpost);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
