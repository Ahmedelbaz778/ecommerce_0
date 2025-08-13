using ECommerceAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtSettings _jwtSettings;
        private readonly Econtext _context;
        private readonly PasswordHasher<Admins> _passwordHasher;

        public AuthController(IOptions<JwtSettings> jwtSettings, Econtext context)
        {
            _jwtSettings = jwtSettings.Value;
            _context = context;
            _passwordHasher = new PasswordHasher<Admins>();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == model.Email);

            if (admin != null)
            {
                var verificationResult = _passwordHasher.VerifyHashedPassword(admin, admin.Password, model.Password);
                if (verificationResult == PasswordVerificationResult.Success)
                {
                    var token = GenerateToken(admin.Email);
                    return Ok(new { Token = token });
                }
            }

            return Unauthorized("Invalid email or password");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var existingAdmin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == model.Email);
            if (existingAdmin != null)
            {
                return BadRequest("Email already exists.");
            }

            var newAdmin = new Admins
            {
                Name = model.Name,
                Email = model.Email,
            };

            newAdmin.Password = _passwordHasher.HashPassword(newAdmin, model.Password);

            await _context.Admins.AddAsync(newAdmin);
            await _context.SaveChangesAsync();

            return Ok("Registration successful.");
        }

        private string GenerateToken(string email)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
