using CarLog.Maintenance.Domain.Entities;

namespace CarLog.Maintenance.Application.Common.Interfaces;

public interface IMaintenanceRepository
{
    Task<MaintenanceEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<MaintenanceEntity>> GetByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken);
    
    Task AddAsync(MaintenanceEntity maintenance, CancellationToken cancellationToken);
    
    void Update(MaintenanceEntity maintenance);

    void Remove(MaintenanceEntity maintenance);
}
