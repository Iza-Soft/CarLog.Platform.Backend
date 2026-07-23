using CarLog.Expense.Application.Common.Exceptions;
using CarLog.Expense.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarLog.Expense.Application.Features.Expenses.Commands.DeleteExpense;

public class DeleteExpenseCommandHandler : IRequestHandler<DeleteExpenseCommand>
{
    private readonly IExpenseRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteExpenseCommandHandler> _logger;

    public DeleteExpenseCommandHandler(IExpenseRepository repository, IUnitOfWork unitOfWork, ILogger<DeleteExpenseCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _repository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException(nameof(Domain.Entities.ExpenseEntity), request.Id);

        _repository.Remove(expense);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Deleted expense {ExpenseId} for vehicle {VehicleId}", request.Id, expense.VehicleId);
    }
}