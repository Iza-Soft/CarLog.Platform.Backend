using CarLog.Maintenance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarLog.Maintenance.Infrastructure.Persistence;

public class MaintenanceDbContext : DbContext
{
    public MaintenanceDbContext(DbContextOptions<MaintenanceDbContext> options) : base(options) { }

    public DbSet<MaintenanceEntity> Maintenances => Set<MaintenanceEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MaintenanceEntity>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.Property(m => m.VehicleId)
                .IsRequired();

            entity.Property(m => m.Type)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(m => m.Description)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(m => m.ServiceDate)
                .IsRequired();

            entity.Property(m => m.MileageAtService)
                .IsRequired();

            entity.OwnsOne(m => m.Cost, cost =>
            {
                cost.Property(c => c.Amount)
                    .HasColumnName("CostAmount")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                cost.Property(c => c.Currency)
                    .HasColumnName("CostCurrency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            entity.Property(m => m.CreatedAtUtc)
                .IsRequired();

            entity.HasIndex(m => m.VehicleId);
        });

        base.OnModelCreating(modelBuilder);
    }
}
