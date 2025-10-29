using System.ComponentModel.DataAnnotations;

namespace RestERP.Application.DTOs
{
    public class RefreshTokenRequest
    {
        [Required(ErrorMessage = "Refresh token zorunludur")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}

