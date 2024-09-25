using akademedya_backend.Data.Context;
using akademedya_backend.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTablesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserTablesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllUsersTables()
        {
            try
            {
                var userTables = await _context.UserTables.ToListAsync();
                return Ok(userTables);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        [HttpGet("getUserTable/{userID}")]
        [Authorize]
        public async Task<IActionResult> GetPersonsTables(int userID)
        {
            try
            {
                var tables = await _context.UserTables.Where(t => t.UserId == userID).ToListAsync();
                if (tables == null || tables.Count == 0)
                    return Ok("Kullanicinin tablosu yok");

                return Ok(tables);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddUserTables(UserTables userTable)
        {
            try
            {
                _context.UserTables.Add(userTable);
                await _context.SaveChangesAsync();
                return Ok(await _context.UserTables.ToListAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }
    }
}
