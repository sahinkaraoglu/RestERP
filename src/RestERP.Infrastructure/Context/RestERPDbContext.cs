using Microsoft.EntityFrameworkCore;
using RestERP.Core.Domain.Entities;
using RestERP.Infrastructure.Data.SeedData;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RestERP.Infrastructure.Context
{
    public class RestERPDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public RestERPDbContext(DbContextOptions<RestERPDbContext> options)
            : base(options)
        {
        }

        public DbSet<Food> Foods { get; set; }
        public DbSet<FoodCategory> FoodCategories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Soft delete filtreleri
            builder.Entity<Order>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<OrderItem>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<Food>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<Table>().HasQueryFilter(e => !e.IsDeleted);

            // Entity konfigürasyonları
            
            // OrderItem konfigürasyonu
            builder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasPrecision(18, 2);
                
            builder.Entity<OrderItem>()
                .Property(oi => oi.TotalPrice)
                .HasPrecision(18, 2);
                
            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);
                
            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Food)
                .WithMany()
                .HasForeignKey(oi => oi.FoodId);
            
            // Order konfigürasyonu
            builder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);
                
            // Order - ApplicationUser ilişkisi
            builder.Entity<Order>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .IsRequired(false);
            
            // RefreshToken - ApplicationUser ilişkisi
            builder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Product konfigürasyonu
            builder.Entity<Food>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);
                
            // Seed verileri
            builder.Entity<FoodCategory>().HasData(FoodCategorySeedData.GetFoodCategories());
            builder.Entity<Food>().HasData(FoodSeedData.GetFood());
            builder.Entity<Table>().HasData(TableSeedData.GetTable());
            builder.Entity<Image>().HasData(ImageSeedData.GetImages());
            // Identity kullanıcı/rol seed'i ayrı bir başlangıç adımında yönetilmelidir
        }
    }
}