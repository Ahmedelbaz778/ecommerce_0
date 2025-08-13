using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly IAdminRepository _repo;

        public AdminsController(IAdminRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admins>>> GetAdmins()
        {
            var admins = await _repo.GetAllAsync();
            return Ok(admins);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Admins>> GetAdmin(int id)
        {
            var admin = await _repo.GetByIdAsync(id);
            if (admin == null)
                return NotFound();

            return Ok(admin);
        }

        [HttpPost]
        public async Task<ActionResult<Admins>> CreateAdmin(Admins admin)
        {
            await _repo.AddAsync(admin);
            var saved = await _repo.SaveChangesAsync();

            if (!saved)
                return StatusCode(500, "Error saving data");

            return CreatedAtAction(nameof(GetAdmin), new { id = admin.Id }, admin);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdmin(int id, Admins admin)
        {
            if (id != admin.Id)
                return BadRequest();

            _repo.Update(admin);

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
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var admin = await _repo.GetByIdAsync(id);
            if (admin == null)
                return NotFound();

            _repo.Delete(admin);
            var saved = await _repo.SaveChangesAsync();

            if (!saved)
                return StatusCode(500, "Failed to delete");

            return NoContent();
        }
    }
}
