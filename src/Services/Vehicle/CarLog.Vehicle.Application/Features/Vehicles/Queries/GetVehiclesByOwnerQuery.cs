using AutoMapper;
using CarLog.Vehicle.Application.DTOs;
using CarLog.Vehicle.Application.Interfaces.Repositories;
using MediatR;

namespace CarLog.Vehicle.Application.Features.Vehicles.Queries;

public class GetVehiclesByOwnerQuery : IRequest<IEnumerable<VehicleDto>>
{
    public Guid OwnerId { get; set; }

    public string OwnerType { get; set; } = null!;
}

public class GetVehiclesByOwnerQueryHandler : IRequestHandler<GetVehiclesByOwnerQuery, IEnumerable<VehicleDto>> 
{
    private readonly IVehicleRepository _vehicleRepository;

    private readonly IMapper _mapper;

    public GetVehiclesByOwnerQueryHandler(IVehicleRepository vehicleRepository, IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;

        _mapper = mapper;
    }

    public async Task<IEnumerable<VehicleDto>> Handle(GetVehiclesByOwnerQuery request, CancellationToken cancellationToken) 
    {
        var vehicles = await _vehicleRepository.GetByOwnerAsync(request.OwnerId, request.OwnerType, cancellationToken);

        return _mapper.Map<IEnumerable<VehicleDto>>(vehicles);
    }
}