using CarLog.Reminder.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarLog.Reminder.Infrastructure.Persistence;

public class ReminderDbContext : DbContext
{
    public ReminderDbContext(DbContextOptions<ReminderDbContext> options) : base(options) { }

    public DbSet<ReminderEntity> Reminders => Set<ReminderEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReminderEntity>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.VehicleId)
                .IsRequired();

            entity.Property(r => r.Type)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(r => r.Description)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(r => r.DueDate);

            entity.Property(r => r.DueMileage);

            entity.Property(r => r.IsCompleted)
                .IsRequired();

            entity.Property(r => r.CreatedAtUtc)
                .IsRequired();

            entity.HasIndex(r => r.VehicleId);

            entity.HasIndex(r => new { r.VehicleId, r.IsCompleted });
        });

        base.OnModelCreating(modelBuilder);
    }
}