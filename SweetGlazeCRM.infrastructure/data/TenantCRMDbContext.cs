using Microsoft.EntityFrameworkCore;
using SweetGlazeCRM.domain.entities;

namespace SweetGlazeCRM.infrastructure.data
{
    public class TenantCRMDbContext : DbContext
    {
        public TenantCRMDbContext(
            DbContextOptions<TenantCRMDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>(entity =>
            {
                entity.HasKey(x => x.ProductId);

                entity.Property(x => x.ProductCode)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(x => x.ProductName)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(x => x.UnitPrice)
                    .HasPrecision(18, 2);

                entity.HasIndex(x => x.ProductCode)
                    .IsUnique();
            });
        }
    }
}