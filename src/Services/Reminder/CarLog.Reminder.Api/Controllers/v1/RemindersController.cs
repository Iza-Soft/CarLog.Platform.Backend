using Asp.Versioning;
using CarLog.Reminder.Application.DTOs;
using CarLog.Reminder.Application.Features.Reminders.Commands.CreateReminder;
using CarLog.Reminder.Application.Features.Reminders.Commands.DeleteReminder;
using CarLog.Reminder.Application.Features.Reminders.Commands.MarkReminderCompleted;
using CarLog.Reminder.Application.Features.Reminders.Commands.UpdateReminder;
using CarLog.Reminder.Application.Features.Reminders.Queries.GetReminderById;
using CarLog.Reminder.Application.Features.Reminders.Queries.GetRemindersByVehicle;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarLog.Reminder.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[Authorize]
public class RemindersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<RemindersController> _logger;

    public RemindersController(IMediator mediator, ILogger<RemindersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get reminder by ID
    /// </summary>
    /// <param name="id">Reminder ID</param>
    /// <returns>Reminder details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ReminderDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetReminderByIdQuery(id));

        return Ok(result);
    }

    /// <summary>
    /// Get all reminders for a specific vehicle
    /// </summary>
    /// <param name="vehicleId">Vehicle ID</param>
    /// <returns>List of reminders, incomplete first, sorted by due date</returns>
    [HttpGet("by-vehicle/{vehicleId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IReadOnlyList<ReminderDto>>> GetByVehicle(Guid vehicleId)
    {
        var result = await _mediator.Send(new GetRemindersByVehicleQuery(vehicleId));

        return Ok(result);
    }

    /// <summary>
    /// Create new reminder
    /// </summary>
    /// <param name="command">Reminder data</param>
    /// <returns>Created reminder</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ReminderDto>> Create([FromBody] CreateReminderCommand command)
    {
        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Update reminder details
    /// </summary>
    /// <param name="id">Reminder ID</param>
    /// <param name="command">Update data</param>
    /// <returns>Updated reminder</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ReminderDto>> Update(Guid id, [FromBody] UpdateReminderCommand command)
    {
        if (id != command.Id)
        {
            _logger.LogWarning("ID mismatch during Update: route ID {RouteId}, body ID {BodyId}", id, command.Id);

            return BadRequest("ID mismatch");
        }

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    /// <summary>
    /// Mark reminder as completed
    /// </summary>
    /// <param name="id">Reminder ID</param>
    /// <returns>Updated reminder</returns>
    [HttpPatch("{id}/complete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ReminderDto>> MarkCompleted(Guid id)
    {
        var result = await _mediator.Send(new MarkReminderCompletedCommand(id));

        return Ok(result);
    }

    /// <summary>
    /// Delete reminder
    /// </summary>
    /// <param name="id">Reminder ID</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteReminderCommand(id));

        return NoContent();
    }
}