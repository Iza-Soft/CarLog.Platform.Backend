using CarLog.Expense.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarLog.Expense.Infrastructure.Persistence;

public class ExpenseDbContext : DbContext
{
    public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options) { }

    public DbSet<ExpenseEntity> Expenses => Set<ExpenseEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExpenseEntity>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.VehicleId)
                .IsRequired();

            entity.Property(e => e.Type)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.ExpenseDate)
                .IsRequired();

            entity.OwnsOne(e => e.Amount, amount =>
            {
                amount.Property(a => a.Amount)
                    .HasColumnName("AmountValue")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                amount.Property(a => a.Currency)
                    .HasColumnName("AmountCurrency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            entity.Property(e => e.CreatedAtUtc)
                .IsRequired();

            entity.HasIndex(e => e.VehicleId);

            entity.HasIndex(e => e.Type);
        });

        base.OnModelCreating(modelBuilder);
    }
}