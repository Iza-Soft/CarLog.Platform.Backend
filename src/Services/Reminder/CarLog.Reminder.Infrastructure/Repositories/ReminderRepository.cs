using CarLog.Reminder.Application.Common.Interfaces;
using CarLog.Reminder.Domain.Entities;
using CarLog.Reminder.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarLog.Reminder.Infrastructure.Repositories;

public class ReminderRepository : IReminderRepository
{
    private readonly ReminderDbContext _context;

    public ReminderRepository(ReminderDbContext context)
    {
        _context = context;
    }

    public async Task<ReminderEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Reminders.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<ReminderEntity>> GetByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken)
    {
        return await _context.Reminders.Where(r => r.VehicleId == vehicleId).OrderBy(r => r.IsCompleted).ThenBy(r => r.DueDate).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ReminderEntity reminder, CancellationToken cancellationToken)
    {
        await _context.Reminders.AddAsync(reminder, cancellationToken);
    }

    public void Update(ReminderEntity reminder)
    {
        _context.Entry(reminder).State = EntityState.Modified;
    }

    public void Remove(ReminderEntity reminder)
    {
        _context.Reminders.Remove(reminder);
    }
}