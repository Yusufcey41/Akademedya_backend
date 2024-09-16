using akademedya_backend.Data.Context;
using akademedya_backend.Data.Models;
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

        public UserTablesController (ApplicationDbContext context)
        {
            _context = context; 
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersTables()
        {
            var UserTables = await _context.UserTables.ToListAsync();
            return Ok(UserTables);
        }

        [HttpGet("{userID}")]
        public async Task<IActionResult> GetPersonsTables(int userID)
        {
            var tables = await _context.UserTables.Where(t => t.UserId == userID).ToListAsync();
            if (tables is null)
                return NotFound("user has not got table");
            return Ok(tables);
        }

        [HttpPost]
        public async Task<IActionResult> addUserTables(UserTables usertable )
        {
            _context.UserTables.Add(usertable);
            await _context.SaveChangesAsync();
            return Ok(await _context.UserTables.ToListAsync());
        }
    }
}
