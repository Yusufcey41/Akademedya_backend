using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using akademedya_backend.Data.Models;
using akademedya_backend.Data.Context;
using Microsoft.EntityFrameworkCore;


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
            var users = await _context.UsersInformations.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.UsersInformations.FindAsync(id);
            if(user is null)
                return NotFound("Kullanıcı Bulunamadı!");
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UsersInformation user)
        {
            _context.UsersInformations.Add(user);
            await _context.SaveChangesAsync();
            return Ok(await _context.UsersInformations.ToListAsync());
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UsersInformation updatedUser)//Swaggera yazcağım ya da işte web isteğinden gelen veri
        {
            var userToUpdate = await _context.UsersInformations.FindAsync(updatedUser.UserId); //veritabanında güncelleyeceğin kayıt usetToUpdate olacak
            if (userToUpdate is null)
                return NotFound("Kullanıcı Bulunamadı!");
            userToUpdate.Firstname = updatedUser.Firstname;
            userToUpdate.Lastname = updatedUser.Lastname;
            userToUpdate.Email = updatedUser.Email;
            //password
            userToUpdate.UserTables = updatedUser.UserTables;
            await _context.SaveChangesAsync();
            return Ok(await _context.UsersInformations.ToListAsync());
        }
    }
}
