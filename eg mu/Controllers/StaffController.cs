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

        // ==========================================
        // 1. إدارة مهامي الشخصية (My Daily Tasks)
        // ==========================================

        [HttpGet("my-tasks")]
        public async Task<IActionResult> GetMyTasks()
        {
            // يجلب كل المهمات لكل الموظفين مباشرة وبدون أي قيود أو فلترة
            var tasks = await _context.DailyTasks
                .Include(t => t.Staff)
                .OrderByDescending(t => t.TaskDate)
                .AsNoTracking()
                .ToListAsync();

            return Ok(new { Status = "Success", Data = tasks });
        }

        [HttpPut("complete-task/{taskId}")]
        public async Task<IActionResult> MarkTaskAsCompleted(int taskId, [FromQuery] int staffId)
        {
            // يستقبل الـ staffId مباشرة من الـ Swagger لتحديد الموظف المسؤول عن المهمة
            var task = await _context.DailyTasks
                .FirstOrDefaultAsync(t => t.TaskId == taskId && t.StaffId == staffId);

            if (task == null) return NotFound(new { Message = "المهمة غير موجودة أو ليست لهذا الموظف" });

            task.IsCompleted = true;
            await _context.SaveChangesAsync();
            return Ok(new { Message = "تم تحديث حالة المهمة بنجاح ✅" });
        }

        // ==========================================
        // 2. الحضور والانصراف (Attendance)
        // ==========================================

        [HttpGet("all-attendance")]
        public async Task<IActionResult> GetAllAttendance()
        {
            // يجلب كل سجلات الحضور لجميع الموظفين في النظام مباشرة
            var attendanceList = await _context.Attendances
                .Include(a => a.Staff)
                .OrderByDescending(a => a.CheckIn)
                .AsNoTracking()
                .ToListAsync();

            return Ok(new { Status = "Success", Count = attendanceList.Count, Data = attendanceList });
        }

        [HttpPost("check-in")]
        public async Task<IActionResult> CheckIn([FromQuery] int staffId)
        {
            // يستقبل الـ staffId مباشرة لتسجيل حضور أي موظف تفرضه في الـ Swagger
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
        public async Task<IActionResult> CheckOut([FromQuery] int staffId)
        {
            // يستقبل الـ staffId مباشرة لتسجيل انصراف الموظف المحدد
            var today = DateTime.Today;

            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(a => a.StaffId == staffId && a.CheckIn >= today && a.CheckOut == null);

            if (attendance == null) return BadRequest(new { Message = "لا يوجد سجل حضور مفتوح حالياً لهذا الموظف" });

            attendance.CheckOut = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok(new { Message = "تم تسجيل الانصراف بنجاح 👋", CheckOutTime = attendance.CheckOut });
        }
    }
}