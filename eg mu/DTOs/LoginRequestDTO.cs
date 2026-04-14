using System.ComponentModel.DataAnnotations;

namespace eg_mu.DTOs
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "الإيميل مطلوب")]
        
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        public string Password { get; set; } = string.Empty;

    }
}
