using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using akademedya_backend.Data.Context;
using akademedya_backend.Data.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        public AuthController(ApplicationDbContext context, IConfiguration configuration, EmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp( [FromBody] UsersInformation user)
        {
            try
            {  
                var existingUser = await _context.UsersInformations
                    .FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                    return BadRequest("Bu e-posta ile zaten bir kullanıcı var!");

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

                var newUser = new UsersInformation
                {
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Email = user.Email,
                    Password = hashedPassword,
                    isActive = false
                };

                _context.UsersInformations.Add(newUser);
                await _context.SaveChangesAsync();

                // Create activation link
                var activationLink = $"http://localhost:5173/activate-account/{newUser.UserId}";

                // Send activation email
                var subject = "Hesap Aktivasyonu";
                var body =  $"Hesabınıza girmek için kullanacağınız id numaranız: {newUser.UserId}  Hesabınızı aktifleştirmek için lütfen şu linke tıklayın: <a href='{activationLink}'>Aktivasyon Linki</a>";
                await _emailService.SendEmailAsync(newUser.Email, subject, body);

                return Ok($"Kayıt başarılı! Aktivasyon e-postasını kontrol edin.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        [HttpPost("activate/{userId}")]
        public async Task<IActionResult> ActivateAccount(string userId)
        {
            try
            {
                //var userId = request.userId.ToString();
                // Kullanıcıyı ID ile bul ve IsActive alanını true yap
                var user = await _context.UsersInformations.FirstOrDefaultAsync(u => u.UserId.ToString() == userId);
                if (user == null)
                    return NotFound("Kullanıcı bulunamadı.");

                if (user.isActive)
                    return BadRequest("Hesap zaten aktif.");

                user.isActive = true;
                await _context.SaveChangesAsync();

                return Ok("Hesabınız başarıyla aktifleştirildi.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu:lalala {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginrequest)
        {
            var kullaniciyok = "yok";
            var passwordfailed = "wrong";
            try
            {
                
                var user = await _context.UsersInformations.FirstOrDefaultAsync(u => u.UserId == loginrequest.userid);
                if (user == null) 
                    return Ok(kullaniciyok);
                
                   

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginrequest.password, user.Password);
                if (!isPasswordValid)
                    return Ok(passwordfailed);

                if (!user.isActive)
                    return Ok(false);

                var token = GenerateJwtToken(user);
                var response = new
                {
                    Token = token,
                    Firstname = user.Firstname ,  
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        private string GenerateJwtToken(UsersInformation user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public class LoginRequest
        {
            public int userid { get; set; }
            public string password { get; set; }
        }
      
    }
}
