using MediatR;
using CarLog.Expense.Application.DTOs;

namespace CarLog.Expense.Application.Features.Expenses.Queries.GetExpenseById;

public sealed record GetExpenseByIdQuery(Guid Id) : IRequest<ExpenseDto>;