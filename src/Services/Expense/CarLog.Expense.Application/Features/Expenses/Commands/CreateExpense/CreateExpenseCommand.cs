using MediatR;
using CarLog.Expense.Application.DTOs;
using CarLog.Expense.Domain.Enums;

namespace CarLog.Expense.Application.Features.Expenses.Commands.CreateExpense;

public sealed record CreateExpenseCommand(
    Guid VehicleId,
    ExpenseType Type,
    string Description,
    DateOnly ExpenseDate,
    decimal AmountValue,
    string AmountCurrency) : IRequest<ExpenseDto>;