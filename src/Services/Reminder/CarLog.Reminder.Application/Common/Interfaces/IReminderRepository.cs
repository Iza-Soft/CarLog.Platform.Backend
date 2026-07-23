using CarLog.Reminder.Domain.Entities;

namespace CarLog.Reminder.Application.Common.Interfaces;

public interface IReminderRepository
{
    Task<ReminderEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<ReminderEntity>> GetByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken);

    Task AddAsync(ReminderEntity reminder, CancellationToken cancellationToken);

    void Update(ReminderEntity reminder);

    void Remove(ReminderEntity reminder);
}