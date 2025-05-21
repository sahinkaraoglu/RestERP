using Microsoft.EntityFrameworkCore;
using RestERP.MenuService.Models;

namespace RestERP.MenuService.Data
{
    public class MenuDbContext : DbContext
    {
        public MenuDbContext(DbContextOptions<MenuDbContext> options) : base(options)
        {
        }

        public DbSet<Food> Foods { get; set; }
        public DbSet<FoodCategory> FoodCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Food entity config
            modelBuilder.Entity<Food>()
                .Property(f => f.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Food>()
                .Property(f => f.Price)
                .IsRequired()
                .HasPrecision(10, 2);

            modelBuilder.Entity<FoodCategory>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Food>()
                .HasOne(f => f.Category)
                .WithMany(c => c.Foods)
                .HasForeignKey(f => f.CategoryId);
        }
    }
} 