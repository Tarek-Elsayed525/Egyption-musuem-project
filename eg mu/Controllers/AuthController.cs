using eg_mu.DTOs;
using eg_mu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eg_mu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly GrandEgyptianContext _context;

        public AuthController(GrandEgyptianContext context)
        {
            _context = context;
        }

        // ============================================================
        // 1. عملية تسجيل الدخول (Login) - فحص الهوية فقط
        // ============================================================
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // متغيرات لتخزين بيانات المستخدم المكتشف
            string role = string.Empty;
            string userName = string.Empty;
            string userId = string.Empty;

            // --- [ فحص الأدمن ] ---
            var admin = await _context.Set<Admin>()
                .FirstOrDefaultAsync(a => a.Email == loginDto.Email && a.PasswordHash == loginDto.Password);

            if (admin != null)
            {
                role = "Admin";
                userName = admin.FullName;
                userId = admin.AdminId.ToString();
            }
            else
            {
                // --- [ فحص الموظفين ] ---
                var staff = await _context.Set<Staff>()
                    .FirstOrDefaultAsync(s => s.Email == loginDto.Email && s.PasswordHash == loginDto.Password);
                if (staff != null)
                {
                    role = "Staff";
                    userName = staff.FullName;
                    userId = staff.StaffId.ToString();
                }
                else
                {
                    // --- [ فحص الزوار ] ---
                    var visitor = await _context.Set<Visitor>()
                        .FirstOrDefaultAsync(v => v.Email == loginDto.Email && v.PasswordHash == loginDto.Password);
                    if (visitor != null)
                    {
                        role = "Visitor";
                        userName = $"{visitor.FirstName} {visitor.LastName}";
                        userId = visitor.VisitorId.ToString();
                    }
                }
            }

            // --- [ الرد النهائي في حالة النجاح ] ---
            if (!string.IsNullOrEmpty(role))
            {
                return Ok(new
                {
                    Status = "Success",
                    Message = $"Welcome back, {userName}",
                    Data = new
                    {
                        // تم إزالة التوكن لأنه غير مفعل حالياً في الـ Program.cs
                        Role = role,
                        DisplayName = userName,
                        UserId = userId,
                        IsAuthenticated = true
                    }
                });
            }

            return Unauthorized(new { Message = "خطأ في البريد الإلكتروني أو كلمة المرور ❌" });
        }

        // ============================================================
        // 2. تسجيل زائر جديد (Register)
        // ============================================================
        [HttpPost("register-visitor")]
        public async Task<IActionResult> RegisterVisitor([FromBody] RegisterVisitorDTO registerDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // التأكد من عدم تكرار الإيميل
            var exists = await _context.Visitors.AnyAsync(v => v.Email == registerDto.Email);
            if (exists) return BadRequest(new { Message = "⚠️ هذا البريد الإلكتروني مسجل بالفعل لدينا." });

            var newVisitor = new Visitor
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PasswordHash = registerDto.Password, // يفضل مستقبلاً استخدام Hashing
                CreatedAt = DateTime.Now
            };

            _context.Visitors.Add(newVisitor);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "تم إنشاء حساب الزائر بنجاح! استمتع برحلتك في GEM ✅" });
        }
    }
}