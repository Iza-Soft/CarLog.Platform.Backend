using AutoMapper;
using MediatR;
using CarLog.Expense.Application.Common.Exceptions;
using CarLog.Expense.Application.Common.Interfaces;
using CarLog.Expense.Application.DTOs;

namespace CarLog.Expense.Application.Features.Expenses.Queries.GetExpenseById;

public class GetExpenseByIdQueryHandler : IRequestHandler<GetExpenseByIdQuery, ExpenseDto>
{
    private readonly IExpenseRepository _repository;
    private readonly IMapper _mapper;

    public GetExpenseByIdQueryHandler(IExpenseRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ExpenseDto> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
    {
        var expense = await _repository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException(nameof(Domain.Entities.ExpenseEntity), request.Id);

        return _mapper.Map<ExpenseDto>(expense);
    }
}