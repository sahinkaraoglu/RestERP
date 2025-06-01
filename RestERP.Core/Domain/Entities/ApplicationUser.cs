using RestERP.Core.Domain.Entities.Base;
using RestERP.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace RestERP.Core.Domain.Entities
{
    public class ApplicationUser : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Phone]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public bool IsActive { get; set; } = true;
        
        public Role RoleType { get; set; } = Role.Customer;
    }
} 