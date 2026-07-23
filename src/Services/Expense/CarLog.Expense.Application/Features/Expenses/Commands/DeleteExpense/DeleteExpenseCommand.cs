using MediatR;

namespace CarLog.Expense.Application.Features.Expenses.Commands.DeleteExpense;

public sealed record DeleteExpenseCommand(Guid Id) : IRequest;