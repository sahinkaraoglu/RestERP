using RestERP.Core.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace RestERP.Core.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public int UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        
        [Required]
        public string Token { get; set; } = string.Empty;
        
        public DateTime ExpiresAt { get; set; }
        
        public bool IsRevoked { get; set; } = false;
        
        public DateTime? RevokedAt { get; set; }
    }
}

