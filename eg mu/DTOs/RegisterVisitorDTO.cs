using System.ComponentModel.DataAnnotations;

namespace eg_mu.DTOs
{
    public class RegisterVisitorDTO
    {
        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "الاسم الأخير مطلوب")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@(gmail\.com|yahoo\.com)$",
        ErrorMessage = "عذراً، يجب استخدام حساب Gmail أو Yahoo فقط")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z0-9]+$",
            ErrorMessage = "يجب أن تحتوي كلمة المرور على أرقام وحروف كبيرة وصغيرة، ولا يسمح بالرموز.")]
        public string Password { get; set; } = null!;

        // الحقل الجديد: تأكيد كلمة المرور
        [Required(ErrorMessage = "يرجى تأكيد كلمة المرور")]
        [Compare("Password", ErrorMessage = "تنبيه: كلمة المرور وتأكيدها غير متطابقين")]
        public string ConfirmPassword { get; set; } = null!;
    }
}