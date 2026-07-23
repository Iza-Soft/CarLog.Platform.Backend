using CarLog.Reminder.Application.Common.Exceptions;
using CarLog.Reminder.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarLog.Reminder.Application.Features.Reminders.Commands.DeleteReminder;

public class DeleteReminderCommandHandler : IRequestHandler<DeleteReminderCommand>
{
    private readonly IReminderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteReminderCommandHandler> _logger;

    public DeleteReminderCommandHandler(IReminderRepository repository, IUnitOfWork unitOfWork, ILogger<DeleteReminderCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(DeleteReminderCommand request, CancellationToken cancellationToken)
    {
        var reminder = await _repository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException(nameof(Domain.Entities.ReminderEntity), request.Id);

        _repository.Remove(reminder);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Deleted reminder {ReminderId} for vehicle {VehicleId}", request.Id, reminder.VehicleId);
    }
}