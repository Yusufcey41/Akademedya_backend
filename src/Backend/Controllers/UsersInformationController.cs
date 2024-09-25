using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using akademedya_backend.Data.Models;
using akademedya_backend.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersInformationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersInformationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _context.UsersInformations.ToListAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await _context.UsersInformations.FindAsync(id);
                if (user == null)
                    return NotFound("Kullanıcı Bulunamadı!");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUser(UsersInformation updatedUser)
        {
            try
            {
                var userToUpdate = await _context.UsersInformations.FindAsync(updatedUser.UserId);
                if (userToUpdate == null)
                    return NotFound("Kullanıcı Bulunamadı!");

                userToUpdate.Firstname = updatedUser.Firstname;
                userToUpdate.Lastname = updatedUser.Lastname;
                userToUpdate.Email = updatedUser.Email;
                userToUpdate.isActive = updatedUser.isActive;
                userToUpdate.UserTables = updatedUser.UserTables;
                await _context.SaveChangesAsync();

                return Ok(await _context.UsersInformations.ToListAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        [HttpDelete("{userid}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int userid)
        {
            try
            {
                var deleteToUser  = await _context.UsersInformations.FindAsync(userid);
                if (deleteToUser == null)
                    return NotFound("user does not exist");
                _context.UsersInformations.Remove(deleteToUser);
                await _context.SaveChangesAsync();
                return Ok("user deleted successfully");
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
           

        }
    }
}
