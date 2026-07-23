using CarLog.Reminder.Application.Common.Interfaces;

namespace CarLog.Reminder.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ReminderDbContext _context;

    public UnitOfWork(ReminderDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}