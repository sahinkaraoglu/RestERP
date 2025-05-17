using Microsoft.EntityFrameworkCore;
using RestERP.Domain.Entities;
using RestERP.Infrastructure.Data.SeedData;

namespace RestERP.Infrastructure
{
    public class RestERPDbContext : DbContext
    {
        public RestERPDbContext(DbContextOptions<RestERPDbContext> options)
            : base(options)
        {
        }

        public DbSet<Food> Foods { get; set; }
        public DbSet<FoodCategory> FoodCategories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Table> Tables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Entity konfigürasyonları burada yapılacak
            
            // OrderItem konfigürasyonu
            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasPrecision(18, 2);
                
            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.TotalPrice)
                .HasPrecision(18, 2);
                
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);
                
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Food)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);
            
            // Order konfigürasyonu
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);
            
            // Product konfigürasyonu
            modelBuilder.Entity<Food>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);
                
            // Seed verilerini ekle
            modelBuilder.Entity<FoodCategory>().HasData(FoodCategorySeedData.GetFoodCategories());
            modelBuilder.Entity<Food>().HasData(FoodSeedData.GetFood());
            modelBuilder.Entity<Table>().HasData(TableSeedData.GetTable());
        }
    }
}