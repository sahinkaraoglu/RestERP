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
        }
    }
}