using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SweetGlazeCRM.domain.Entities;
using SweetGlazeCRM.infrastructure.data.Configuration;

namespace SweetGlazeCRM.infrastructure.data
{
    public class MasterCRMDbContext : IdentityDbContext
    {
        public MasterCRMDbContext(
            DbContextOptions<MasterCRMDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies => Set<Company>();

        public DbSet<CompanyDatabase> CompanyDatabases => Set<CompanyDatabase>();

        public DbSet<Device> Devices => Set<Device>();

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Company
            builder.Entity<Company>(entity =>
            {
                entity.HasKey(x => x.CompanyId);

                entity.Property(x => x.CompanyCode)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(x => x.CompanyName)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.HasIndex(x => x.CompanyCode)
                    .IsUnique();
            });

            // CompanyDatabase
            builder.Entity<CompanyDatabase>(entity =>
            {
                entity.HasKey(x => x.CompanyDatabaseId);

                entity.Property(x => x.ServerName)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(x => x.DatabaseName)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.HasOne(x => x.Company)
                    .WithMany()
                    .HasForeignKey(x => x.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Device
            builder.Entity<Device>(entity =>
            {
                entity.HasKey(x => x.DeviceId);

                entity.Property(x => x.DeviceCode)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(x => x.DeviceName)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.HasOne(x => x.Company)
                    .WithMany(x => x.Devices)
                    .HasForeignKey(x => x.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(x => new { x.CompanyId, x.DeviceCode })
                    .IsUnique();
            });

            // Customer
            builder.ApplyConfiguration(new CustomerConfiguration());
        }
    }
}