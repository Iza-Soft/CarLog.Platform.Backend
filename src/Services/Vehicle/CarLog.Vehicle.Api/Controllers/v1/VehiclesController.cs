using CarLog.Vehicle.Application.Features.Vehicles.Commands;
using CarLog.Vehicle.Application.Features.Vehicles.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarLog.Vehicle.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class VehiclesController : ControllerBase
{
    private readonly IMediator _mediator;
    
    private readonly ILogger<VehiclesController> _logger;

    public VehiclesController(IMediator mediator, ILogger<VehiclesController> logger)
    {
        _mediator = mediator;

        _logger = logger;
    }

    /// <summary>
    /// Get vehicle by ID
    /// </summary>
    /// <param name="id">Vehicle ID</param>
    /// <returns>Vehicle details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetById(Guid id) 
    {
        var query = new GetVehicleByIdQuery { Id = id };

        var result = await _mediator.Send(query);

        if (result == null) return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Get vehicles by owner (B2C or B2B)
    /// </summary>
    /// <param name="ownerId">Owner ID (User ID or Company ID)</param>
    /// <param name="ownerType">Owner type (Personal or Corporate)</param>
    /// <returns>List of vehicles</returns>
    [HttpGet("by-owner/{ownerId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetByOwner(Guid ownerId, [FromQuery] string ownerType) 
    {
        var query = new GetVehiclesByOwnerQuery
        {
            OwnerId = ownerId,

            OwnerType = ownerType
        };

        var result = await _mediator.Send(query);

        return Ok(result);
    }

    /// <summary>
    /// Create new vehicle
    /// </summary>
    /// <param name="command">Vehicle data</param>
    /// <returns>Created vehicle</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create([FromBody] CreateVehicleCommand command)
    {
        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Update vehicle details
    /// </summary>
    /// <param name="id">Vehicle ID</param>
    /// <param name="command">Update data</param>
    /// <returns>Updated vehicle</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateVehicleCommand command)
    {
        if (id != command.Id) return BadRequest("ID mismatch");

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    /// <summary>
    /// Update vehicle mileage
    /// </summary>
    /// <param name="id">Vehicle ID</param>
    /// <param name="command">Mileage data</param>
    /// <returns>Updated vehicle</returns>
    [HttpPatch("{id}/mileage")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateMileage(Guid id, [FromBody] UpdateMileageCommand command)
    {
        if (id != command.Id) return BadRequest("ID mismatch");

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    /// <summary>
    /// Soft delete vehicle
    /// </summary>
    /// <param name="id">Vehicle ID</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid id)
    {
        var command = new DeleteVehicleCommand { Id = id };

        await _mediator.Send(command);
        
        return NoContent();
    }
}
