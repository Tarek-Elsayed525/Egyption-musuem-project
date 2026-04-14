using eg_mu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eg_mu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class MuseumController : ControllerBase
    {
        private readonly GrandEgyptianContext _context;

        public MuseumController(GrandEgyptianContext context)
        {
            _context = context;
        }

        // 1. المرحلة الأولى: عرض قائمة بكل القاعات (الأسماء والوصف المختصر)
        // هذه الصفحة التي يراها المستخدم أول ما يدخل قسم المتحف
        [HttpGet("list-sections")]
        public async Task<IActionResult> GetSectionsList()
        {
            var sections = await _context.MuseumSections
                .Select(s => new {
                    s.SectionId,
                    s.Name,
                    s.Description
                })
                .ToListAsync();

            return Ok(sections);
        }

        // 2. المرحلة الثانية: عند الضغط على قاعة معينة (Explore Section)
        // تعرض وصف القاعة بالتفصيل ثم قائمة القطع الأثرية التي بداخلها
        [HttpGet("explore/{sectionId}")]
        public async Task<IActionResult> ExploreSection(int sectionId)
        {
            // جلب القاعة مع القطع الأثرية المرتبطة بها (Include)
            var section = await _context.MuseumSections
                .Include(s => s.Artifacts)
                .FirstOrDefaultAsync(s => s.SectionId == sectionId);

            if (section == null)
            {
                return NotFound(new { Message = "عذراً، هذه القاعة غير موجودة حالياً" });
            }

            // ترتيب النتيجة ليظهر وصف القاعة في البداية ثم المصفوفة (Array) الخاصة بالآثار
            var result = new
            {
                // بيانات القاعة (تظهر في أعلى الصفحة)
                Header = new
                {
                    Title = section.Name,
                    DetailedDescription = section.Description
                },
                // بيانات الآثار (تظهر أسفل الوصف)
                Artifacts = section.Artifacts.Select(a => new {
                    a.ArtifactId,
                    a.Name,
                    a.Era,
                    a.Material,
                    a.Description,
                    a.ImageUrl
                }).ToList()
            };

            return Ok(result);
        }
    }
}