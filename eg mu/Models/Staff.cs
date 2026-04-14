using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace eg_mu.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    [Required(ErrorMessage = "الاسم بالكامل مطلوب")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    // إضافة الـ Regex اللي طلبته هنا
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@(gmail\.com|yahoo\.com)$",
        ErrorMessage = "يجب استخدام Gmail أو Yahoo فقط")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "كلمة المرور مطلوبة")]
    public string PasswordHash { get; set; } = null!;

    public string? Position { get; set; }

    public int? SectionId { get; set; }

    public DateTime? HireDate { get; set; }

    // إضافة JsonIgnore وعلامة الاستفهام لمنع أخطاء الـ Validation والعلاقات الدائرية
    [JsonIgnore]
    public virtual ICollection<Attendance>? Attendances { get; set; } = new List<Attendance>();

    [JsonIgnore]
    public virtual ICollection<DailyTask>? DailyTasks { get; set; } = new List<DailyTask>();

    [JsonIgnore]
    public virtual MuseumSection? Section { get; set; }
}