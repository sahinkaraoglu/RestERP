using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RestERP.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
} 