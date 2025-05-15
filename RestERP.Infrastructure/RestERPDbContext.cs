using Microsoft.EntityFrameworkCore;
using RestERP.Domain.Entities;

namespace RestERP.Infrastructure
{
    public class RestERPDbContext : DbContext
    {
        public RestERPDbContext(DbContextOptions<RestERPDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
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
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);
            
            // Order konfigürasyonu
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);
            
            // Product konfigürasyonu
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);
        }
    }
}