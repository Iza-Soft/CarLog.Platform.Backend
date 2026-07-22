using CarLog.Vehicle.Application.Interfaces;
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

    public async Task<IEnumerable<VehicleEntity>> GetByOwnerAsync(Guid ownerId, OwnerType ownerType, CancellationToken cancellationToken = default)
    {
        return await _context.Vehicles.Where(v => v.OwnerId == ownerId && v.OwnerType == ownerType).ToListAsync(cancellationToken);
    }

    public async Task<VehicleEntity> AddAsync(VehicleEntity vehicle, CancellationToken cancellationToken = default)
    {
        await _context.Vehicles.AddAsync(vehicle, cancellationToken);

        return vehicle;
    }

    public Task UpdateAsync(VehicleEntity vehicle, CancellationToken cancellationToken = default)
    {
        _context.Entry(vehicle).State = EntityState.Modified;
        
        return Task.CompletedTask;
    }

    public Task DeleteAsync(VehicleEntity vehicle, CancellationToken cancellationToken = default)
    {
        _context.Vehicles.Remove(vehicle);
        
        return Task.CompletedTask;
    }

    public async Task<bool> LicensePlateExistsAsync(string licensePlate, string countryCode, CancellationToken cancellationToken = default)
    {
        return await _context.Vehicles.AnyAsync(v => v.LicensePlate.PlateNumber == licensePlate && v.LicensePlate.CountryCode == countryCode, cancellationToken);
    }
}