using CarLog.Expense.Application.Common.Interfaces;
using CarLog.Expense.Domain.Entities;
using CarLog.Expense.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarLog.Expense.Infrastructure.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly ExpenseDbContext _context;

    public ExpenseRepository(ExpenseDbContext context)
    {
        _context = context;
    }

    public async Task<ExpenseEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Expenses.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<ExpenseEntity>> GetByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken)
    {
        return await _context.Expenses.Where(e => e.VehicleId == vehicleId).OrderByDescending(e => e.ExpenseDate).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ExpenseEntity expense, CancellationToken cancellationToken)
    {
        await _context.Expenses.AddAsync(expense, cancellationToken);
    }

    public void Update(ExpenseEntity expense)
    {
        _context.Entry(expense).State = EntityState.Modified;
    }

    public void Remove(ExpenseEntity expense)
    {
        _context.Expenses.Remove(expense);
    }
}