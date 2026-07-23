using CarLog.Expense.Application.Common.Interfaces;

namespace CarLog.Expense.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ExpenseDbContext _context;

    public UnitOfWork(ExpenseDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}