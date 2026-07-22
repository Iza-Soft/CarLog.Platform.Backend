using CarLog.Maintenance.Application.Common.Interfaces;
using CarLog.Maintenance.Domain.Entities;
using CarLog.Maintenance.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarLog.Maintenance.Infrastructure.Repositories;

public class MaintenanceRepository : IMaintenanceRepository
{
    private readonly MaintenanceDbContext _context;

    public MaintenanceRepository(MaintenanceDbContext context) 
    {
        _context = context;
    }

    public async Task<MaintenanceEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken) 
    {
        return await _context.Maintenances.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<MaintenanceEntity>> GetByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken) 
    {
        return await _context.Maintenances.Where(m => m.VehicleId == vehicleId).OrderByDescending(m => m.ServiceDate).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(MaintenanceEntity maintenance, CancellationToken cancellationToken) 
    {
        await _context.Maintenances.AddAsync(maintenance, cancellationToken);
    }

    public void Update(MaintenanceEntity maintenance) 
    {
        _context.Entry(maintenance).State = EntityState.Modified;
    }

    public void Remove(MaintenanceEntity maintenance) 
    {
        _context.Maintenances.Remove(maintenance);
    }
}
