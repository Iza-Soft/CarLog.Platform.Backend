using AutoMapper;
using CarLog.Expense.Application.Common.Exceptions;
using CarLog.Expense.Application.Common.Interfaces;
using CarLog.Expense.Application.DTOs;
using CarLog.Expense.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarLog.Expense.Application.Features.Expenses.Commands.UpdateExpense;

public class UpdateExpenseCommandHandler : IRequestHandler<UpdateExpenseCommand, ExpenseDto>
{
    private readonly IExpenseRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateExpenseCommandHandler> _logger;

    public UpdateExpenseCommandHandler(IExpenseRepository repository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateExpenseCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ExpenseDto> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _repository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException(nameof(Domain.Entities.ExpenseEntity), request.Id);

        var amount = Money.Create(request.AmountValue, request.AmountCurrency);

        expense.Update(request.Type, request.Description, request.ExpenseDate, amount);

        _repository.Update(expense);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updated expense {ExpenseId} for vehicle {VehicleId}, new type {Type}, new amount {AmountValue} {AmountCurrency}", expense.Id, expense.VehicleId, request.Type, request.AmountValue, request.AmountCurrency);

        return _mapper.Map<ExpenseDto>(expense);
    }
}