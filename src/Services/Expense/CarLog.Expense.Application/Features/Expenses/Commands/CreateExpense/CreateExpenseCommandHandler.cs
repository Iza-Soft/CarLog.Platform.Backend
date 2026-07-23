using AutoMapper;
using CarLog.Expense.Application.Common.Interfaces;
using CarLog.Expense.Application.DTOs;
using CarLog.Expense.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarLog.Expense.Application.Features.Expenses.Commands.CreateExpense;

public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, ExpenseDto>
{
    private readonly IExpenseRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateExpenseCommandHandler> _logger;

    public CreateExpenseCommandHandler(IExpenseRepository repository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateExpenseCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ExpenseDto> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var amount = Money.Create(request.AmountValue, request.AmountCurrency);

        var expense = Domain.Entities.ExpenseEntity.Create(
            request.VehicleId,
            request.Type,
            request.Description,
            request.ExpenseDate,
            amount);

        await _repository.AddAsync(expense, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created expense {ExpenseId} for vehicle {VehicleId}, type {Type}, amount {AmountValue} {AmountCurrency}", expense.Id, request.VehicleId, request.Type, request.AmountValue, request.AmountCurrency);

        return _mapper.Map<ExpenseDto>(expense);
    }
}