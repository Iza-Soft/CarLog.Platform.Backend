using CarLog.Expense.Domain.Enums;
using CarLog.Expense.Domain.Exceptions;
using CarLog.Expense.Domain.ValueObjects;

namespace CarLog.Expense.Domain.Entities;

public sealed class ExpenseEntity
{
    public Guid Id { get; private set; }

    public Guid VehicleId { get; private set; }

    public ExpenseType Type { get; private set; }

    public string Description { get; private set; } = string.Empty;

    public DateOnly ExpenseDate { get; private set; }

    public Money Amount { get; private set; } = null!;

    public DateTime CreatedAtUtc { get; private set; }

    private ExpenseEntity() { }

    public static ExpenseEntity Create(Guid vehicleId, ExpenseType type, string description, DateOnly expenseDate, Money amount)
    {
        if (vehicleId == Guid.Empty) throw new DomainException("VehicleId is required.");

        if (string.IsNullOrWhiteSpace(description)) throw new DomainException("Description cannot be empty.");

        if (expenseDate > DateOnly.FromDateTime(DateTime.UtcNow)) throw new DomainException("Expense date cannot be in the future.");

        return new ExpenseEntity
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicleId,
            Type = type,
            Description = description.Trim(),
            ExpenseDate = expenseDate,
            Amount = amount,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public void Update(ExpenseType type, string description, DateOnly expenseDate, Money amount)
    {
        if (string.IsNullOrWhiteSpace(description)) throw new DomainException("Description cannot be empty.");

        if (expenseDate > DateOnly.FromDateTime(DateTime.UtcNow)) throw new DomainException("Expense date cannot be in the future.");

        Type = type;
        Description = description.Trim();
        ExpenseDate = expenseDate;
        Amount = amount;
    }
}