namespace CarLog.Expense.Application.DTOs;

public sealed record ExpenseDto(
    Guid Id,
    Guid VehicleId,
    string Type,
    string Description,
    DateOnly ExpenseDate,
    decimal AmountValue,
    string AmountCurrency,
    DateTime CreatedAtUtc);