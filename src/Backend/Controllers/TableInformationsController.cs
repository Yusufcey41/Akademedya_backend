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
    public class TableInformationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TableInformationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllTables()
        {
            try
            {
                var tables = await _context.TableInformations.ToListAsync();
                return Ok(tables);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        
        [HttpGet("gettable/{id}")]
        [Authorize]
        public async Task<IActionResult> GetTable(int id)
        {
            try
            {
                var table = await _context.TableInformations.FindAsync(id);
                if (table == null)
                    return NotFound("Tablo mevcut değil.");
                return Ok(table);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }



        
        [HttpPost("addtable/{UserId}")]
        [Authorize]
        public async Task<IActionResult> AddTable(TableInformations table, int UserId)
        {
            try
            {
                string destinationPath = Path.Combine("C:\\Akademedya_frontend\\frontend\\public\\images", Path.GetFileName(table.ImageUrl));

                // Dosyayı kopyala
                System.IO.File.Copy(table.ImageUrl, destinationPath);
                table.ImageUrl = "\\images\\"+ Path.GetFileName(table.ImageUrl);

                _context.TableInformations.Add(table);
                await _context.SaveChangesAsync();

                var userTable = new UserTables
                {
                    UserId = UserId,
                    TableId = table.TableId
                };
                _context.UserTables.Add(userTable);
                await _context.SaveChangesAsync();

                return Ok(table);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateTable(TableInformations UpdateTable)
        {
            try
            {
                var tableToUpdate = await _context.TableInformations.FindAsync(UpdateTable.TableId);
                if (tableToUpdate == null)
                    return NotFound("Tablo mevcut değil.");

                tableToUpdate.TableName = UpdateTable.TableName;
                tableToUpdate.ImageUrl = UpdateTable.ImageUrl;

                // Foreign key tabloları güncelleme
                tableToUpdate.Columns = UpdateTable.Columns;
                tableToUpdate.UserTables = UpdateTable.UserTables;
                tableToUpdate.TablesValue = UpdateTable.TablesValue;

                await _context.SaveChangesAsync();
                return Ok(await _context.TableInformations.ToListAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        
        [HttpDelete("{tableid}")]
        [Authorize]
        public async Task<IActionResult> DeleteTable(int tableid)
        {
            try
            {
                var table = await _context.TableInformations.FindAsync(tableid);
                if (table == null) return NotFound("table does not exist");
                _context.TableInformations.Remove(table);
                await _context.SaveChangesAsync();
                return Ok("table deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"bir hata oluştu  {ex.Message}");
            }
        }
    }
}
