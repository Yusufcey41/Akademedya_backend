using akademedya_backend.Data.Context;
using akademedya_backend.Data.Models;
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
        public async Task<IActionResult> GetColumns()
        {
            var columns = await _context.Columns.ToListAsync();
            return Ok(columns);
        }


        [HttpPost]
        public async Task<IActionResult> AddColumn(Columns columns)
        {
            _context.Columns.Add(columns);
            await _context.SaveChangesAsync();
            return Ok(await _context.Columns.ToListAsync());
        }


        [HttpGet("tablescolumn/{tableid}")]
         public async Task<IActionResult> GetTablesColumns(int tableid)
         {
            var columns = await _context.Columns.Where(t=> t.TableId == tableid).ToListAsync();
            Console.WriteLine($"Number of columns retrieved: {columns.Count}");
            if (columns is null)
                return NotFound("table has not got columns");
            return Ok(columns);
         }


        [HttpGet("{ColumnId}")]
        public async Task<IActionResult> GetColumn(int ColumnId)
        {
            var column = await _context.Columns.Where(t => t.ColumnId == ColumnId).ToListAsync();
            if (column is null)
                return NotFound("table has not got columns");
            return Ok(column);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateColumm( Columns updatedcolumn, int tableid)
        {
            var columnToUpdate = await _context.Columns.Where(t => t.ColumnId == updatedcolumn.ColumnId && t.TableId == tableid).FirstOrDefaultAsync();
            if (columnToUpdate is null)
                return NotFound("column is not exist");

            columnToUpdate.ColumnName = updatedcolumn.ColumnName;
            columnToUpdate.TableInformations = updatedcolumn.TableInformations;
            await _context.SaveChangesAsync();
            return Ok(await _context.Columns.ToListAsync());
        }
    }
}
