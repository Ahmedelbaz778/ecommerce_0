using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;

        public ProductsController(IProductRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> GetProducts()
        {
            var products = await _repo.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> GetProduct(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Products>> PostProduct(Products product)
        {
            await _repo.AddAsync(product);
            var saved = await _repo.SaveChangesAsync();

            if (!saved)
                return StatusCode(500, "Failed to save product");

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Products product)
        {
            if (id != product.Id)
                return BadRequest();

            _repo.Update(product);

            try
            {
                var saved = await _repo.SaveChangesAsync();
                if (!saved)
                    return StatusCode(500, "Failed to update");
            }
            catch
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            _repo.Delete(product);
            var saved = await _repo.SaveChangesAsync();

            if (!saved)
                return StatusCode(500, "Failed to delete");

            return NoContent();
        }

        [HttpGet("trending")]
        public async Task<ActionResult<IEnumerable<object>>> GetTrendingProducts()
        {
            var trending = await _repo.GetTrendingAsync();
            var result = trending.Select(p => new
            {
                p.Id,
                p.Name,
                p.Price,
                p.ImageUrl
            });

            return Ok(result);
        }
    }
}
