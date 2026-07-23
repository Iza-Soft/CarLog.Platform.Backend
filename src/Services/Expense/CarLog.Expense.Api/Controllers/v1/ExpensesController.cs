using Asp.Versioning;
using CarLog.Expense.Application.DTOs;
using CarLog.Expense.Application.Features.Expenses.Commands.CreateExpense;
using CarLog.Expense.Application.Features.Expenses.Commands.DeleteExpense;
using CarLog.Expense.Application.Features.Expenses.Commands.UpdateExpense;
using CarLog.Expense.Application.Features.Expenses.Queries.GetExpenseById;
using CarLog.Expense.Application.Features.Expenses.Queries.GetExpensesByVehicle;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarLog.Expense.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[Authorize]
public class ExpensesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ExpensesController> _logger;

    public ExpensesController(IMediator mediator, ILogger<ExpensesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get expense record by ID
    /// </summary>
    /// <param name="id">Expense record ID</param>
    /// <returns>Expense record details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ExpenseDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetExpenseByIdQuery(id));

        return Ok(result);
    }

    /// <summary>
    /// Get all expense records for a specific vehicle
    /// </summary>
    /// <param name="vehicleId">Vehicle ID</param>
    /// <returns>List of expense records</returns>
    [HttpGet("by-vehicle/{vehicleId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IReadOnlyList<ExpenseDto>>> GetByVehicle(Guid vehicleId)
    {
        var result = await _mediator.Send(new GetExpensesByVehicleQuery(vehicleId));

        return Ok(result);
    }

    /// <summary>
    /// Create new expense record
    /// </summary>
    /// <param name="command">Expense record data</param>
    /// <returns>Created expense record</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ExpenseDto>> Create([FromBody] CreateExpenseCommand command)
    {
        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Update expense record details
    /// </summary>
    /// <param name="id">Expense record ID</param>
    /// <param name="command">Update data</param>
    /// <returns>Updated expense record</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ExpenseDto>> Update(Guid id, [FromBody] UpdateExpenseCommand command)
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
    /// Delete expense record
    /// </summary>
    /// <param name="id">Expense record ID</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteExpenseCommand(id));

        return NoContent();
    }
}