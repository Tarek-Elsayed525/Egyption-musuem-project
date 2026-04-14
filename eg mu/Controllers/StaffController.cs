using eg_mu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eg_mu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class StaffController : ControllerBase
    {
        private readonly GrandEgyptianContext _context;

        public StaffController(GrandEgyptianContext context)
        {
            _context = context;
        }

        // دالة مؤقتة لتجربة البيانات (حط الـ ID بتاع أي موظف عندك في الداتابيز هنا)
        private int GetUserId()
        {
            // هنفترض إنك بتجرب بموظف رقمه 1.. غير الرقم ده براحتك حسب الداتا اللي عندك
            return 1;
        }

        // ==========================================
        // 1. إدارة مهامي الشخصية (My Daily Tasks)
        // ==========================================

        [HttpGet("my-tasks")]
        public async Task<IActionResult> GetMyTasks()
        {
            int staffId = GetUserId();
            var tasks = await _context.DailyTasks
                .Where(t => t.StaffId == staffId)
                .OrderByDescending(t => t.TaskDate)
                .AsNoTracking()
                .ToListAsync();

            return Ok(new { Status = "Success", Data = tasks });
        }

        [HttpPut("complete-task/{taskId}")]
        public async Task<IActionResult> MarkTaskAsCompleted(int taskId)
        {
            int staffId = GetUserId();
            var task = await _context.DailyTasks
                .FirstOrDefaultAsync(t => t.TaskId == taskId && t.StaffId == staffId);

            if (task == null) return NotFound(new { Message = "المهمة غير موجودة أو ليست ملكك" });

            task.IsCompleted = true;
            await _context.SaveChangesAsync();
            return Ok(new { Message = "تم تحديث حالة المهمة بنجاح ✅" });
        }

        // ==========================================
        // 2. الحضور والانصراف (Attendance)
        // ==========================================

        [HttpPost("check-in")]
        public async Task<IActionResult> CheckIn()
        {
            int staffId = GetUserId();
            var today = DateTime.Today;

            var alreadyCheckedIn = await _context.Attendances
                .AnyAsync(a => a.StaffId == staffId && a.CheckIn >= today);

            if (alreadyCheckedIn) return BadRequest(new { Message = "لقد سجلت حضورك بالفعل اليوم" });

            var attendance = new Attendance
            {
                StaffId = staffId,
                CheckIn = DateTime.Now
            };

            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "تم تسجيل الحضور بنجاح ⏱️", CheckInTime = attendance.CheckIn });
        }

        [HttpPut("check-out")]
        public async Task<IActionResult> CheckOut()
        {
            int staffId = GetUserId();
            var today = DateTime.Today;

            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(a => a.StaffId == staffId && a.CheckIn >= today && a.CheckOut == null);

            if (attendance == null) return BadRequest(new { Message = "لا يوجد سجل حضور مفتوح حالياً" });

            attendance.CheckOut = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok(new { Message = "تم تسجيل الانصراف بنجاح 👋", CheckOutTime = attendance.CheckOut });
        }
    }
}