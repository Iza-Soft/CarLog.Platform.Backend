using CarLog.Vehicle.Application.Interfaces.Repositories;
using CarLog.Vehicle.Domain.Entities;
using CarLog.Vehicle.Domain.Enums;
using CarLog.Vehicle.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarLog.Vehicle.Infrastructure.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly VehicleDbContext _context;

    public VehicleRepository(VehicleDbContext context) 
    {
        _context = context;
    }

    public async Task<VehicleEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<VehicleEntity>> GetByOwnerAsync(Guid ownerId, string ownerType, CancellationToken cancellationToken = default)
    {
        var ownerTypeEnum = Enum.Parse<OwnerType>(ownerType);

        return await _context.Vehicles.Where(v => v.OwnerId == ownerId && v.OwnerType == ownerTypeEnum).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<VehicleEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Vehicles.ToListAsync(cancellationToken);
    }

    public async Task<VehicleEntity> AddAsync(VehicleEntity vehicle, CancellationToken cancellationToken = default)
    {
        await _context.Vehicles.AddAsync(vehicle, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return vehicle;
    }

    public async Task UpdateAsync(VehicleEntity vehicle, CancellationToken cancellationToken = default)
    {
        _context.Entry(vehicle).State = EntityState.Modified;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(VehicleEntity vehicle, CancellationToken cancellationToken = default)
    {
        _context.Vehicles.Remove(vehicle);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Vehicles.AnyAsync(v => v.Id == id, cancellationToken);
    }

    public async Task<bool> LicensePlateExistsAsync(string licensePlate, string countryCode, CancellationToken cancellationToken = default)
    {
        return await _context.Vehicles.AnyAsync(v => v.LicensePlate.PlateNumber == licensePlate && v.LicensePlate.CountryCode == countryCode, cancellationToken);
    }
}
