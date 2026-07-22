using FluentValidation;

namespace CarLog.Maintenance.Application.Features.Maintenances.Commands.DeleteMaintenance;

public class DeleteMaintenanceCommandValidator : AbstractValidator<DeleteMaintenanceCommand>
{
    public DeleteMaintenanceCommandValidator() 
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}
