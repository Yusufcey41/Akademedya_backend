using akademedya_backend.Data.Context;
using akademedya_backend.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableInformationsController : ControllerBase
    {


        private readonly ApplicationDbContext _context;

        public TableInformationsController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllTables()
        {
            var tables = await _context.TableInformations.ToListAsync();
            return Ok(tables);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTable(int id)
        {
            var table = await _context.TableInformations.FindAsync(id);
            if(table is null)
                return NotFound("user is not available");
            return Ok(table);

        }
        [HttpPost]
        public async Task<IActionResult> AddTable(TableInformations table , int UserId)
        {
            _context.TableInformations.Add(table);             
            await _context.SaveChangesAsync();
            var userTable = new UserTables
            {
                UserId = UserId,
                TableId = table.TableId 
            };
            _context.UserTables.Add(userTable);
            await _context.SaveChangesAsync();

            return Ok( await _context.TableInformations.ToListAsync());
        }
        [HttpPut]
        public async Task<IActionResult> UpdateTable(TableInformations UpdateTable)
        {
            var tableToUpdate = await _context.TableInformations.FindAsync(UpdateTable.TableId);
            if(UpdateTable is null)
                return NotFound("table is not available");
            
            tableToUpdate.TableName = UpdateTable.TableName;
            tableToUpdate.ImageUrl = UpdateTable.ImageUrl;

            //foreign key tabloları
            tableToUpdate.Columns = UpdateTable.Columns;
            tableToUpdate.UserTables = UpdateTable.UserTables;
            tableToUpdate.TablesValue = UpdateTable.TablesValue;

            await _context.SaveChangesAsync();
            return Ok(await _context.TableInformations.ToListAsync());

        }
    }
}
