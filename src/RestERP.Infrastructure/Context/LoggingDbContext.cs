using Microsoft.EntityFrameworkCore;
using RestERP.Core.Domain.Entities;

namespace RestERP.Infrastructure.Context
{
    public class LoggingDbContext : DbContext
    {
        public LoggingDbContext(DbContextOptions<LoggingDbContext> options)
            : base(options)
        {
        }

        public DbSet<RequestLog> RequestLogs => Set<RequestLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RequestLog>(entity =>
            {
                entity.ToTable("RequestLogs");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RequestName).HasMaxLength(256).IsRequired();
                entity.Property(e => e.RequestType).HasMaxLength(32).IsRequired();
                entity.Property(e => e.RequestPayload).HasMaxLength(4000);
                entity.Property(e => e.ResponsePayload).HasMaxLength(4000);
                entity.Property(e => e.ErrorMessage).HasMaxLength(2000);
                entity.Property(e => e.UserId).HasMaxLength(64);
                entity.Property(e => e.UserName).HasMaxLength(256);
                entity.Property(e => e.CorrelationId).HasMaxLength(128);
                entity.HasIndex(e => e.Timestamp);
                entity.HasIndex(e => e.RequestName);
            });
        }
    }
}
