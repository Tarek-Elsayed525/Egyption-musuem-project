using eg_mu.Models; // تأكد أن هذا هو اسم المجلد الذي يحتوي على الموديلات لديك
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eg_mu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ExhibitionController : ControllerBase
    {
        private readonly GrandEgyptianContext _context;

        public ExhibitionController(GrandEgyptianContext context)
        {
            _context = context;
        }

        // 1. عرض قائمة بكل المعارض (للمستخدمين)
        // GET: api/Exhibition
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Exhibition>>> GetAllExhibitions()
        {
            // يجلب المعارض مع بيانات القسم (Section) التابعة له
            return await _context.Exhibitions
                .Include(e => e.Section)
                .ToListAsync();
        }

        // 2. عرض تفاصيل معرض محدد والقطع الأثرية التي بداخله (عند الضغط عليه)
        // GET: api/Exhibition/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Exhibition>> GetExhibitionDetails(int id)
        {
            var exhibition = await _context.Exhibitions
                .Include(e => e.Section) // عرض القسم التابع له
                .Include(e => e.ArtifactsExhibitions) // عرض جدول الربط بين المعرض والقطع
                    .ThenInclude(ae => ae.Artifact) // عرض بيانات القطعة الأثرية نفسها
                .FirstOrDefaultAsync(e => e.ExhibitionId == id);

            if (exhibition == null)
            {
                return NotFound("المعرض غير موجود");
            }

            return exhibition;
        }
    }
}