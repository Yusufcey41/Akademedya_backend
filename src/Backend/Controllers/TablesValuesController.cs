using akademedya_backend.Data.Context;
using akademedya_backend.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablesValuesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TablesValuesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetValues()
        {
            try
            {
                var values = await _context.TablesValues.ToListAsync();
                return Ok(values);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        [HttpPost("addrows")]
        [Authorize]
        public async Task<IActionResult> AddValue(TablesValues value)
        {
            try
            {
                _context.TablesValues.Add(value);
                await _context.SaveChangesAsync();
                return Ok(value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        [HttpGet("{valueid}")]
        [Authorize]
        public async Task<IActionResult> GetValue(int valueid)
        {
            try
            {
                var value = await _context.TablesValues.Where(t => t.InputAreaId == valueid).ToListAsync();
                if (value == null || value.Count == 0)
                    return NotFound("Bu değer alanı mevcut değil.");
                return Ok(value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }


        [HttpPut("decreaseinputareid/{tableid}/{inputAreaId}/{azalmamiktarı}")]
        [Authorize]
        public async Task<IActionResult> DecreaseId(int tableId, int inputAreaId, int azalmamiktarı)
        {
            try
            {
                // Güncellenecek verileri al
                var valuesToUpdate = await _context.TablesValues
                                                     .Where(t => t.InputAreaId > inputAreaId && t.TableId == tableId)
                                                     .ToListAsync();

                // Güncellenecek veri yoksa "yok" mesajını döndür
                if (valuesToUpdate == null )
                    return Ok("yok");

                // Yeni değerleri saklamak için bir liste oluştur
                var newValues = new List<TablesValues>();

                // Silme işlemi
                foreach (var value in valuesToUpdate)
                {
                    // Yeni InputAreaId'yi hesapla
                    int newInputAreaId = value.InputAreaId - azalmamiktarı;

                    // Yeni kaydı oluştur
                    var newValue = new TablesValues
                    {
                        TableId = tableId,
                        InputAreaId = newInputAreaId,
                        Value = value.Value // mevcut değeri kopyala
                    };

                    // Yeni değeri listeye ekle
                    newValues.Add(newValue);

                    // Mevcut kaydı sil
                    _context.TablesValues.Remove(value);
                }

                // Değişiklikleri kaydet
                await _context.SaveChangesAsync();

                // Yeni kayıtları veritabanına ekle
                foreach (var value in newValues)
                {
                     _context.TablesValues.Add(value);
                }
                    

                // Yeni kayıtları kaydet
                await _context.SaveChangesAsync();

                // Güncellenmiş değerleri geri döndür
                return Ok(newValues);
            }
            catch (Exception ex)
            {
                // Hata durumunda 500 hatası döndür
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }






        [HttpGet("tableRows/{tableid}")]
        [Authorize]
        public async Task<IActionResult> GetRows(int tableid)
        {
            try
            {
                var value = await _context.TablesValues.Where(t => t.TableId == tableid).ToListAsync();
                if (value == null || value.Count == 0)
                    return NotFound("Bu tablo satırları mevcut değil.");
                return Ok(value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        [HttpPut("updatevalue")]
        [Authorize]
        public async Task<IActionResult> UpdateValue(TablesValues updatedValue)
        {
            try
            {
                var updateToValue = await _context.TablesValues.FirstOrDefaultAsync(t => t.InputAreaId == updatedValue.InputAreaId && t.TableId == updatedValue.TableId);
                if (updateToValue == null)
                    return NotFound("Değer mevcut değil.");

                updateToValue.Value = updatedValue.Value;
                updateToValue.TableInformations = updatedValue.TableInformations;
                await _context.SaveChangesAsync();
                return Ok(updateToValue);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }


        [HttpDelete("deletevalue")]
        [Authorize]
        public async Task<IActionResult> DeleteValue([FromBody] TablesValues deletevalue )
        {
            try
            {
                var deleteToValue = await _context.TablesValues.FirstOrDefaultAsync(t => t.InputAreaId == deletevalue.InputAreaId && t.TableId == deletevalue.TableId);
                if (deleteToValue == null)
                    return Ok("value does not exist");
                _context.TablesValues.Remove(deleteToValue);
                await _context.SaveChangesAsync();
                return Ok(deleteToValue);

            }

            catch (Exception ex)
            {
                return StatusCode(500, $"bir hata oluştu {ex.Message}");
            }
        }
    }
}
