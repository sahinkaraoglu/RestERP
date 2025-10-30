using RestERP.Core.Domain.Entities;
using RestERP.Domain.Enums;

namespace RestERP.Infrastructure.Data.SeedData
{
    /// <summary>
    /// Identity kullanıcıları için seed verisi: Roller bazında örnek kullanıcılar
    /// Şifreler runtime'da Program.cs içinde UserManager ile atanır:
    /// - admin@resterp.com -> Admin123!
    /// - employee@resterp.com -> Employee123!
    /// - customer@test.com -> Customer123!
    /// </summary>
    public class UserSeedData
    {
        public static List<ApplicationUser> GetUsers()
        {
            return new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    FirstName = "Admin",
                    LastName = "User",
                    UserName = "admin",
                    Email = "admin@resterp.com",
                    PhoneNumber = "05551234567",
                    IsActive = true,
                    RoleType = Role.Admin
                },
                new ApplicationUser
                {
                    FirstName = "Personel",
                    LastName = "User",
                    UserName = "employee",
                    Email = "employee@resterp.com",
                    PhoneNumber = "05559876543",
                    IsActive = true,
                    RoleType = Role.Employee
                },
                new ApplicationUser
                {
                    FirstName = "Test",
                    LastName = "Customer",
                    UserName = "customer",
                    Email = "customer@test.com",
                    PhoneNumber = "05551111111",
                    IsActive = true,
                    RoleType = Role.Customer
                }
            };
        }
    }
}
