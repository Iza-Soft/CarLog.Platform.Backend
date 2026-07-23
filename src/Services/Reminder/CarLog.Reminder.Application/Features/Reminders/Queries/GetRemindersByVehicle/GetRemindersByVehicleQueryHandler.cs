using AutoMapper;
using MediatR;
using CarLog.Reminder.Application.Common.Interfaces;
using CarLog.Reminder.Application.DTOs;

namespace CarLog.Reminder.Application.Features.Reminders.Queries.GetRemindersByVehicle;

public class GetRemindersByVehicleQueryHandler : IRequestHandler<GetRemindersByVehicleQuery, IReadOnlyList<ReminderDto>>
{
    private readonly IReminderRepository _repository;
    private readonly IMapper _mapper;

    public GetRemindersByVehicleQueryHandler(IReminderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ReminderDto>> Handle(GetRemindersByVehicleQuery request, CancellationToken cancellationToken)
    {
        var reminders = await _repository.GetByVehicleIdAsync(request.VehicleId, cancellationToken);

        return _mapper.Map<IReadOnlyList<ReminderDto>>(reminders);
    }
}