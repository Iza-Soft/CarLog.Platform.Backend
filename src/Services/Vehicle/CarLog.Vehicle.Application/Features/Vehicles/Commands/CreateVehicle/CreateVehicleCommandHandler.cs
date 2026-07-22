using AutoMapper;
using CarLog.Vehicle.Application.Common.Exceptions;
using CarLog.Vehicle.Application.Common.Interfaces;
using CarLog.Vehicle.Application.DTOs;
using CarLog.Vehicle.Application.Interfaces;
using MediatR;

namespace CarLog.Vehicle.Application.Features.Vehicles.Commands.CreateVehicle;

public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, VehicleDto>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateVehicleCommandHandler(IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<VehicleDto> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var exists = await _vehicleRepository.LicensePlateExistsAsync(request.LicensePlate, request.CountryCode, cancellationToken);

        if (exists) throw new ConflictException("Vehicle with this license plate already exists");

        var vehicle = Domain.Entities.VehicleEntity.Create(
            request.Make,
            request.Model,
            request.Year,
            request.LicensePlate,
            request.CountryCode,
            request.Vin,
            request.Type,
            request.FuelType,
            request.EngineDisplacement,
            request.HorsePower,
            request.OwnerType,
            request.OwnerId);

        var result = await _vehicleRepository.AddAsync(vehicle, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<VehicleDto>(result);
    }
}