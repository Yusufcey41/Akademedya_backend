using akademedya_backend.Data.Context;
using akademedya_backend.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablesValuesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TablesValuesController (ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetValues()
        {
            var values = await _context.TablesValues.ToListAsync();
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> AddValue(TablesValues value)
        {
            _context.TablesValues.Add(value);
            _context.SaveChanges();
            return Ok(await _context.TablesValues.ToListAsync());
        }

        [HttpGet("{valueid}")]
        public async Task<IActionResult> GetValue(int valueid)
        {
            var value = await _context.TablesValues.Where(t => t.InputAreaId == valueid).ToListAsync();
            if(value is null)
                return NotFound("this value area does not exist");
            return Ok(value);
        }

        [HttpGet("tableRows/{tableid}")]
        public async Task<IActionResult> GetRows(int tableid)
        {
            var value = await _context.TablesValues.Where( t => t.TableId == tableid).ToListAsync();
            if (value is null)
                return NotFound("this value area does not exist");
            return Ok(value);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateValue(TablesValues updatedValue, int tableid)
        {
            var updateToValue = await _context.TablesValues.FirstOrDefaultAsync(t => t.InputAreaId == updatedValue.InputAreaId && t.TableId == tableid);
            if (updateToValue is null)
                return NotFound("value does not exist");
            updateToValue.Value = updatedValue.Value;
            updateToValue.TableInformations = updatedValue.TableInformations;
            await _context.SaveChangesAsync();
            return Ok(await _context.TablesValues.ToListAsync());
        }


    }
}
