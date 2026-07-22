using AutoMapper;
using CarLog.Vehicle.Application.DTOs;
using CarLog.Vehicle.Application.Interfaces;
using MediatR;

namespace CarLog.Vehicle.Application.Features.Vehicles.Queries.GetVehiclesByOwner;

public class GetVehiclesByOwnerQueryHandler : IRequestHandler<GetVehiclesByOwnerQuery, IReadOnlyList<VehicleDto>>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IMapper _mapper;

    public GetVehiclesByOwnerQueryHandler(IVehicleRepository vehicleRepository, IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<VehicleDto>> Handle(GetVehiclesByOwnerQuery request, CancellationToken cancellationToken)
    {
        var vehicles = await _vehicleRepository.GetByOwnerAsync(request.OwnerId, request.OwnerType, cancellationToken);

        return _mapper.Map<IReadOnlyList<VehicleDto>>(vehicles);
    }
}