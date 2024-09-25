using akademedya_backend.Data.Context;
using akademedya_backend.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColumnsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ColumnsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetColumns()
        {
            try
            {
                var columns = await _context.Columns.ToListAsync();
                return Ok(columns);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        [HttpPost("addcolumn")]
        [Authorize]
        public async Task<IActionResult> AddColumn(Columns columns)
        {
            try
            {
                _context.Columns.Add(columns);
                await _context.SaveChangesAsync();
                return Ok(await _context.Columns.ToListAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        [HttpGet("getcolumnname/{tableid}")]
        [Authorize]
        public async Task<IActionResult> GetTablesColumns(int tableid)
        {
            try
            {
                var columns = await _context.Columns.Where(t => t.TableId == tableid).ToListAsync();
                Console.WriteLine($"Number of columns retrieved: {columns.Count}");
                if (columns == null || !columns.Any())
                    return NotFound("Tablonun sütunları yok.");
                return Ok(columns);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        [HttpGet("{ColumnId}")]
        [Authorize]
        public async Task<IActionResult> GetColumn(int ColumnId)
        {
            try
            {
                var column = await _context.Columns.Where(t => t.ColumnId == ColumnId).ToListAsync();
                if (column == null || !column.Any())
                    return NotFound("Sütun bulunamadı.");
                return Ok(column);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateColumm(Columns updatedcolumn, int tableid)
        {
            try
            {
                var columnToUpdate = await _context.Columns
                    .Where(t => t.ColumnId == updatedcolumn.ColumnId && t.TableId == tableid)
                    .FirstOrDefaultAsync();

                if (columnToUpdate == null)
                    return NotFound("Sütun mevcut değil.");

                columnToUpdate.ColumnName = updatedcolumn.ColumnName;
                columnToUpdate.TableInformations = updatedcolumn.TableInformations;

                await _context.SaveChangesAsync();
                return Ok(await _context.Columns.ToListAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }
    }
}
