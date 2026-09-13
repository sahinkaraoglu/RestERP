using System.ComponentModel.DataAnnotations;

namespace RestERP.Application.DTOs
{
    public class ResetPasswordRequest
    {
        [Required(ErrorMessage = "Yeni şifre zorunludur")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre tekrarı zorunludur")]
        [Compare("NewPassword", ErrorMessage = "Şifreler eşleşmiyor")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
