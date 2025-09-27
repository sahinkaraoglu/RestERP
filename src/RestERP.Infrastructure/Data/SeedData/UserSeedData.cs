using RestERP.Core.Domain.Entities;
using RestERP.Domain.Enums;
using System.Security.Cryptography;
using System.Text;

namespace RestERP.Infrastructure.Data.SeedData
{
    public class UserSeedData
    {
        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
        public static List<ApplicationUser> GetUsers()
        {
            return new List<ApplicationUser>
            {
                // Admin Kullanıcısı
                new ApplicationUser
                {
                    Id = 1,
                    FirstName = "Admin",
                    LastName = "User",
                    UserName = "admin",
                    Email = "admin@resterp.com",
                    PhoneNumber = "05551234567",
                    PasswordHash = HashPassword("Admin123!"), // Şifre: Admin123!
                    IsActive = true,
                    RoleType = Role.Employee,
                    IsDeleted = false,
                    CreatedById = null,
                    CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedById = null,
                    UpdatedDate = null
                },
                
                // Employee Kullanıcısı
                new ApplicationUser
                {
                    Id = 2,
                    FirstName = "Çalışan",
                    LastName = "User",
                    UserName = "employee",
                    Email = "employee@resterp.com",
                    PhoneNumber = "05559876543",
                    PasswordHash = HashPassword("password"), // Şifre: password
                    IsActive = true,
                    RoleType = Role.Employee,
                    IsDeleted = false,
                    CreatedById = null,
                    CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedById = null,
                    UpdatedDate = null
                },
                
                // Test Müşteri Kullanıcısı
                new ApplicationUser
                {
                    Id = 3,
                    FirstName = "Test",
                    LastName = "Customer",
                    UserName = "customer",
                    Email = "customer@test.com",
                    PhoneNumber = "05551111111",
                    PasswordHash = HashPassword("password"), // Şifre: password
                    IsActive = true,
                    RoleType = Role.Customer,
                    IsDeleted = false,
                    CreatedById = null,
                    CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedById = null,
                    UpdatedDate = null
                }
            };
        }
    }
}
