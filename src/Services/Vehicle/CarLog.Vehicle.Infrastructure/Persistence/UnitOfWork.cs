using CarLog.Vehicle.Application.Common.Interfaces;

namespace CarLog.Vehicle.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly VehicleDbContext _context;

    public UnitOfWork(VehicleDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}