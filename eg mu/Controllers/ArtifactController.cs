using eg_mu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eg_mu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ArtifactController : ControllerBase
    {
        private readonly GrandEgyptianContext _context;

        public ArtifactController(GrandEgyptianContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. جلب كل القطع الأثرية (مع بيانات القاعة)
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> GetAllArtifacts()
        {
            var artifacts = await _context.Artifacts
                .Include(a => a.Section) // ربط البيانات مع القاعات المخصصة
                .AsNoTracking()
                .ToListAsync();

            return Ok(new
            {
                Status = "Success",
                TotalCount = artifacts.Count,
                Data = artifacts
            });
        }

        // ==========================================
        // 2. البحث عن قطعة معينة بالـ ID
        // ==========================================
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetArtifactById(int id)
        {
            var artifact = await _context.Artifacts
                .Include(a => a.Section)
                .FirstOrDefaultAsync(a => a.ArtifactId == id);

            if (artifact == null)
                return NotFound(new { Message = "القطعة المطلوبة غير موجودة في السجل" });

            return Ok(new
            {
                Status = "Success",
                Data = artifact
            });
        }

        // ==========================================
        // 3. البحث المتقدم باسم القطعة (Search)
        // ==========================================
        [HttpGet("search")]
        public async Task<IActionResult> SearchByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { Message = "يرجى إدخال اسم القطعة المراد البحث عنها" });

            var artifacts = await _context.Artifacts
                .Include(a => a.Section)
                .Where(a => a.Name.Contains(name))
                .AsNoTracking()
                .ToListAsync();

            if (!artifacts.Any())
                return NotFound(new { Message = "لا توجد نتائج تطابق هذا البحث" });

            return Ok(new
            {
                Status = "Success",
                ResultsFound = artifacts.Count,
                Data = artifacts
            });
        }
    }
}