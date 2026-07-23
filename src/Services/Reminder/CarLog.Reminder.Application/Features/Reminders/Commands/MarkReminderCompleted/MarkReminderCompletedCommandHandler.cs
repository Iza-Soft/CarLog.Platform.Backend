using AutoMapper;
using CarLog.Reminder.Application.Common.Exceptions;
using CarLog.Reminder.Application.Common.Interfaces;
using CarLog.Reminder.Application.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarLog.Reminder.Application.Features.Reminders.Commands.MarkReminderCompleted;

public class MarkReminderCompletedCommandHandler : IRequestHandler<MarkReminderCompletedCommand, ReminderDto>
{
    private readonly IReminderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<MarkReminderCompletedCommandHandler> _logger;

    public MarkReminderCompletedCommandHandler(IReminderRepository repository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<MarkReminderCompletedCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ReminderDto> Handle(MarkReminderCompletedCommand request, CancellationToken cancellationToken)
    {
        var reminder = await _repository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException(nameof(Domain.Entities.ReminderEntity), request.Id);

        reminder.MarkCompleted();

        _repository.Update(reminder);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Marked reminder {ReminderId} as completed for vehicle {VehicleId}", reminder.Id, reminder.VehicleId);

        return _mapper.Map<ReminderDto>(reminder);
    }
}