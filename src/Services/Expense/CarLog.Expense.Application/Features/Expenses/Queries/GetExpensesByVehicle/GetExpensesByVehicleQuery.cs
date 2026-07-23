using MediatR;
using CarLog.Expense.Application.DTOs;

namespace CarLog.Expense.Application.Features.Expenses.Queries.GetExpensesByVehicle;

public sealed record GetExpensesByVehicleQuery(Guid VehicleId) : IRequest<IReadOnlyList<ExpenseDto>>;