using RestERP.Core.Domain.Entities;
using RestERP.Domain.Enums;

namespace RestERP.Infrastructure.Data.SeedData
{
    public class UserSeedData
    {
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
                    PasswordHash = "PrP+ZrMeO00Q+nC1ytSccRIpSvauTkdqHEBRVdRaoSE=", // Şifre: Admin123!
                    IsActive = true,
                    RoleType = Role.Employee,
                    CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
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
                    PasswordHash = "sQnzu7wkTrgkQZF+0G1hi5AI3Qmzvv0bXgc5THBqi7m=", // Şifre: password
                    IsActive = true,
                    RoleType = Role.Employee,
                    CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
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
                    PasswordHash = "sQnzu7wkTrgkQZF+0G1hi5AI3Qmzvv0bXgc5THBqi7m=", // Şifre: password
                    IsActive = true,
                    RoleType = Role.Customer,
                    CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            };
        }
    }
}
