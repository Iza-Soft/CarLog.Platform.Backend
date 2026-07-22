using CarLog.Maintenance.Application.Common.Exceptions;
using CarLog.Maintenance.Application.Common.Interfaces;
using CarLog.Maintenance.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarLog.Maintenance.Application.Features.Maintenances.Commands.DeleteMaintenance;

public class DeleteMaintenanceCommandHandler : IRequestHandler<DeleteMaintenanceCommand>
{
    private readonly IMaintenanceRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteMaintenanceCommandHandler> _logger;

    public DeleteMaintenanceCommandHandler(IMaintenanceRepository repository, IUnitOfWork unitOfWork, ILogger<DeleteMaintenanceCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(DeleteMaintenanceCommand request, CancellationToken cancellationToken) 
    {
        var maintenanceEntity = await _repository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException(nameof(MaintenanceEntity), request.Id);

        _repository.Remove(maintenanceEntity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Deleted maintenance record {MaintenanceId} for car {VehicleId}", request.Id, maintenanceEntity.VehicleId);
    }
}
