using CarLog.Vehicle.Domain.Entities;
using CarLog.Vehicle.Domain.Enums;

namespace CarLog.Vehicle.Application.Interfaces;

public interface IVehicleRepository
{
    Task<VehicleEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<VehicleEntity>> GetByOwnerAsync(Guid ownerId, OwnerType ownerType, CancellationToken cancellationToken = default);

    Task<VehicleEntity> AddAsync(VehicleEntity vehicle, CancellationToken cancellationToken = default);

    Task UpdateAsync(VehicleEntity vehicle, CancellationToken cancellationToken = default);

    Task DeleteAsync(VehicleEntity vehicle, CancellationToken cancellationToken = default);

    Task<bool> LicensePlateExistsAsync(string licensePlate, string countryCode, CancellationToken cancellationToken = default);
}