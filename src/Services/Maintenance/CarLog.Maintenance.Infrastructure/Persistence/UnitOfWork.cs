using CarLog.Maintenance.Application.Common.Interfaces;

namespace CarLog.Maintenance.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly MaintenanceDbContext _context;

    public UnitOfWork(MaintenanceDbContext context) 
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken) 
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
