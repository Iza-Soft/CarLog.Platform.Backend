using CarLog.Vehicle.Domain.Entities;

namespace CarLog.Vehicle.Application.Interfaces.Repositories;

public interface IVehicleRepository
{
    Task<VehicleEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<VehicleEntity>> GetByOwnerAsync(Guid ownerId, string ownerType, CancellationToken cancellationToken = default);

    Task<IEnumerable<VehicleEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<VehicleEntity> AddAsync(VehicleEntity vehicle, CancellationToken cancellationToken = default);

    Task UpdateAsync(VehicleEntity vehicle, CancellationToken cancellationToken = default);

    Task DeleteAsync(VehicleEntity vehicle, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> LicensePlateExistsAsync(string licensePlate, string countryCode, CancellationToken cancellationToken = default);
}
