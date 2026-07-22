using MediatR;

namespace CarLog.Vehicle.Application.Features.Vehicles.Commands.DeleteVehicle;

public sealed record DeleteVehicleCommand(Guid Id) : IRequest;