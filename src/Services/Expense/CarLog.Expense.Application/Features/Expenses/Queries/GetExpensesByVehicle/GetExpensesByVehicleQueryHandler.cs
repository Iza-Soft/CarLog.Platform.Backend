using AutoMapper;
using MediatR;
using CarLog.Expense.Application.Common.Interfaces;
using CarLog.Expense.Application.DTOs;

namespace CarLog.Expense.Application.Features.Expenses.Queries.GetExpensesByVehicle;

public class GetExpensesByVehicleQueryHandler : IRequestHandler<GetExpensesByVehicleQuery, IReadOnlyList<ExpenseDto>>
{
    private readonly IExpenseRepository _repository;
    private readonly IMapper _mapper;

    public GetExpensesByVehicleQueryHandler(IExpenseRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ExpenseDto>> Handle(GetExpensesByVehicleQuery request, CancellationToken cancellationToken)
    {
        var expenses = await _repository.GetByVehicleIdAsync(request.VehicleId, cancellationToken);

        return _mapper.Map<IReadOnlyList<ExpenseDto>>(expenses);
    }
}