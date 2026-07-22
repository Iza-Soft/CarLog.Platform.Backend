using Asp.Versioning;
using CarLog.Maintenance.Application.DTOs;
using CarLog.Maintenance.Application.Features.Maintenances.Commands.CreateMaintenancе;
using CarLog.Maintenance.Application.Features.Maintenances.Commands.DeleteMaintenance;
using CarLog.Maintenance.Application.Features.Maintenances.Commands.UpdateMaintenance;
using CarLog.Maintenance.Application.Features.Maintenances.Queries.GetMaintenanceById;
using CarLog.Maintenance.Application.Features.Maintenances.Queries.GetMaintenancesByVehicle;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarLog.Maintenance.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[Authorize]
public class MaintenancesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<MaintenancesController> _logger;

    public MaintenancesController(IMediator mediator, ILogger<MaintenancesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get maintenance record by ID
    /// </summary>
    /// <param name="id">Maintenance record ID</param>
    /// <returns>Maintenance record details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<MaintenanceDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetMaintenanceByIdQuery(id));

        return Ok(result);
    }

    /// <summary>
    /// Get all maintenance records for a specific vehicle
    /// </summary>
    /// <param name="vehicleId">Vehicle ID</param>
    /// <returns>List of maintenance records</returns>
    [HttpGet("by-vehicle/{vehicleId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IReadOnlyList<MaintenanceDto>>> GetByVehicle(Guid vehicleId)
    {
        var result = await _mediator.Send(new GetMaintenancesByVehicleQuery(vehicleId));

        return Ok(result);
    }

    /// <summary>
    /// Create new maintenance record
    /// </summary>
    /// <param name="command">Maintenance record data</param>
    /// <returns>Created maintenance record</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<MaintenanceDto>> Create([FromBody] CreateMaintenanceCommand command)
    {
        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Update maintenance record details
    /// </summary>
    /// <param name="id">Maintenance record ID</param>
    /// <param name="command">Update data</param>
    /// <returns>Updated maintenance record</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<MaintenanceDto>> Update(Guid id, [FromBody] UpdateMaintenanceCommand command)
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
    /// Delete maintenance record
    /// </summary>
    /// <param name="id">Maintenance record ID</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteMaintenanceCommand(id));

        return NoContent();
    }
}
