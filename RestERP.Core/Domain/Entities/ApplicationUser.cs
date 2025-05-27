using Microsoft.AspNetCore.Identity;
using RestERP.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace RestERP.Core.Doman.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        public Role RoleType { get; set; } = Role.Customer;
    }
} 