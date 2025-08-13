using ECommerceAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    public class UserAuthController : ControllerBase
    {
        private readonly JwtSettings _jwtSettings;
        private readonly Econtext _context;
        private readonly PasswordHasher<Users> _passwordHasher;

        public UserAuthController(IOptions<JwtSettings> jwtSettings, Econtext context)
        {
            _jwtSettings = jwtSettings.Value;
            _context = context;
            _passwordHasher = new PasswordHasher<Users>();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user != null)
            {
                var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);
                if (verificationResult == PasswordVerificationResult.Success)
                {
                    var token = GenerateToken(user.Email, user.Role);
                    return Ok(new { Token = token });
                }
            }

            return Unauthorized("Invalid email or password");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterModel model)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (existingUser != null)
            {
                return BadRequest("Email already exists.");
            }

            var newUser = new Users
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Role = model.Role ?? "User" // Default role to 'User' لو مش اتبعت رول
            };

            newUser.Password = _passwordHasher.HashPassword(newUser, model.Password);

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return Ok("Registration successful.");
        }

        private string GenerateToken(string email, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
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
        [HttpGet("me")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null) return Unauthorized("Invalid token");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null) return NotFound("User not found");

           
            var nameParts = user.Name.Split(' ', 2);
            string firstName = nameParts.Length > 0 ? nameParts[0] : user.Name;
            string lastName = nameParts.Length > 1 ? nameParts[1] : "";

            return Ok(new
            {
                firstName = firstName,
                lastName = lastName,
                email = user.Email,
                phone = user.Phone,
                role = user.Role,
                
                registrationDate = DateTime.Now.ToString("MMM dd, yyyy"), 
                image = "/images/default-avatar.png" // لو عندك صورة في الـ DB استبدل هنا
            });
        }


    }

}

    public class UserLoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class UserRegisterModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Password { get; set; }

        public string? Role { get; set; } // Optional
    }



