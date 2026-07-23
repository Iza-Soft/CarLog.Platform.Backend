using CarLog.Expense.Domain.Entities;

namespace CarLog.Expense.Application.Common.Interfaces;

public interface IExpenseRepository
{
    Task<ExpenseEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<ExpenseEntity>> GetByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken);

    Task AddAsync(ExpenseEntity expense, CancellationToken cancellationToken);

    void Update(ExpenseEntity expense);

    void Remove(ExpenseEntity expense);
}