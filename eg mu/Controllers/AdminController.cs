using eg_mu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eg_mu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // تم إزالة [Authorize] لضمان وصولك الكامل لبيانات الإدارة بدون توكن
    public class AdminController : ControllerBase
    {
        private readonly GrandEgyptianContext _context;

        public AdminController(GrandEgyptianContext context)
        {
            _context = context;
        }

        // دالة مساعدة معدلة: بما أننا بدون توكن، سنرجع اسم افتراضي
        private string GetCurrentAdminName() => "GEM Manager (Dev Mode)";

        // ==========================================
        // 1. إدارة الموظفين (Staff Management)
        // ==========================================

        [HttpGet("staff")]
        public async Task<IActionResult> GetAllStaff()
        {
            var staffList = await _context.Staff
                .Include(s => s.Section)
                .AsNoTracking()
                .ToListAsync();

            return Ok(new
            {
                Status = "Success",
                Mode = "Open Access",
                Message = $"Logged in as: {GetCurrentAdminName()}",
                Count = staffList.Count,
                Data = staffList
            });
        }

        [HttpPost("staff")]
        public async Task<IActionResult> AddStaff([FromBody] Staff newStaff)
        {
            if (newStaff == null) return BadRequest("بيانات الموظف غير مكتملة");

            newStaff.StaffId = 0; // لضمان أن SQL يولد ID جديد
            var emailExists = await _context.Staff.AnyAsync(s => s.Email == newStaff.Email);
            if (emailExists) return BadRequest(new { Message = "⚠️ هذا البريد الإلكتروني مسجل مسبقاً لموظف آخر" });

            _context.Staff.Add(newStaff);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "تم إضافة الموظف بنجاح ✅", StaffID = newStaff.StaffId });
        }

        [HttpPut("staff/{id}")]
        public async Task<IActionResult> UpdateStaff(int id, [FromBody] Staff updatedStaff)
        {
            var existingStaff = await _context.Staff.FindAsync(id);
            if (existingStaff == null) return NotFound(new { Message = "❌ الموظف غير موجود" });

            existingStaff.FullName = updatedStaff.FullName;
            existingStaff.Position = updatedStaff.Position;
            existingStaff.SectionId = updatedStaff.SectionId;
            existingStaff.Email = updatedStaff.Email;

            await _context.SaveChangesAsync();
            return Ok(new { Message = "تم تحديث بيانات الموظف بنجاح ✅" });
        }

        [HttpDelete("staff/{id}")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            var staff = await _context.Staff.FindAsync(id);
            if (staff == null) return NotFound(new { Message = "❌ الموظف غير موجود" });

            _context.Staff.Remove(staff);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "تم حذف الموظف وكافة سجلاته بنجاح ✅" });
        }

        // ==========================================
        // 2. إدارة سجلات الحضور (Attendance Management)
        // ==========================================

        [HttpGet("attendance")]
        public async Task<IActionResult> GetAttendanceReport()
        {
            var report = await _context.Attendances
                .Include(a => a.Staff)
                .OrderByDescending(a => a.CheckIn)
                .AsNoTracking()
                .ToListAsync();

            return Ok(new
            {
                Status = "Success",
                ReportDate = DateTime.Now.ToShortDateString(),
                Data = report
            });
        }

        [HttpPost("attendance")]
        public async Task<IActionResult> AddAttendanceManual([FromBody] Attendance attendance)
        {
            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "تم تسجيل سجل الحضور يدوياً بنجاح ✅" });
        }

        [HttpPut("attendance/checkout/{staffId}")]
        public async Task<IActionResult> CheckOut(int staffId)
        {
            var today = DateTime.Today;
            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(a => a.StaffId == staffId && a.CheckIn >= today && a.CheckOut == null);

            if (attendance == null)
                return BadRequest(new { Message = "⚠️ لا يوجد سجل حضور مفتوح لهذا الموظف اليوم" });

            attendance.CheckOut = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok(new { Message = "تم تسجيل وقت الانصراف بنجاح ✅" });
        }

        [HttpDelete("attendance/{id}")]
        public async Task<IActionResult> DeleteAttendance(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null) return NotFound(new { Message = "❌ السجل غير موجود" });

            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "تم حذف سجل الحضور من النظام ✅" });
        }

        // ==========================================
        // 3. إدارة المهمات (Task Management)
        // ==========================================

        [HttpGet("tasks")]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _context.DailyTasks
                .Include(t => t.Staff)
                .Select(t => new {
                    t.TaskId,
                    t.TaskDescription,
                    t.IsCompleted,
                    t.TaskDate,
                    t.StaffId,
                    StaffName = t.Staff != null ? t.Staff.FullName : "غير معين"
                })
                .AsNoTracking()
                .ToListAsync();

            return Ok(new { Status = "Success", Count = tasks.Count, Data = tasks });
        }

        [HttpPost("tasks")]
        public async Task<IActionResult> AddTask([FromBody] DailyTask newTask)
        {
            if (string.IsNullOrEmpty(newTask.TaskDescription)) return BadRequest("وصف المهمة مطلوب");

            _context.DailyTasks.Add(newTask);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "تم إسناد المهمة للموظف بنجاح ✅" });
        }

        [HttpDelete("tasks/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.DailyTasks.FindAsync(id);
            if (task == null) return NotFound(new { Message = "❌ المهمة غير موجودة" });

            _context.DailyTasks.Remove(task);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "تم حذف المهمة بنجاح ✅" });
        }
    }
}