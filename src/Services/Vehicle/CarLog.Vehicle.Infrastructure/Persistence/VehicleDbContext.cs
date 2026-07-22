using CarLog.Vehicle.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarLog.Vehicle.Infrastructure.Persistence;

public class VehicleDbContext : DbContext
{
    public VehicleDbContext(DbContextOptions<VehicleDbContext> options) : base(options) { }

    public DbSet<VehicleEntity> Vehicles => Set<VehicleEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VehicleEntity>(entity =>
        {
            entity.HasKey(v => v.Id);

            entity.Property(v => v.Make)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(v => v.Model)
                .IsRequired()
                .HasMaxLength(50);

            entity.OwnsOne(v => v.Year, year =>
            {
                year.Property(y => y.Value)
                    .HasColumnName("Year")
                    .IsRequired();
            });

            entity.OwnsOne(v => v.LicensePlate, plate =>
            {
                plate.Property(p => p.PlateNumber)
                    .HasColumnName("LicensePlate")
                    .IsRequired()
                    .HasMaxLength(10);

                plate.Property(p => p.CountryCode)
                    .HasColumnName("CountryCode")
                    .IsRequired()
                    .HasMaxLength(2);

                plate.HasIndex(p => new { p.PlateNumber, p.CountryCode })
                    .IsUnique();
            });

            entity.Property(v => v.Vin)
                .HasMaxLength(17);

            entity.Property(v => v.Type)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(v => v.FuelType)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(v => v.EngineDisplacement)
                .IsRequired();

            entity.Property(v => v.HorsePower)
                .IsRequired();

            entity.Property(v => v.OwnerType)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(v => v.OwnerId);

            entity.Property(v => v.CurrentMileage)
                .IsRequired();

            entity.Property(v => v.CreatedAtUtc)
                .IsRequired();

            entity.Property(v => v.UpdatedAtUtc)
                .IsRequired();

            entity.HasIndex(v => v.OwnerId);
        });

        base.OnModelCreating(modelBuilder);
    }
}