using Bombers_System.Application.UseCases.VehicleUseCases.Commands;
using Bombers_System.Application.UseCases.VehicleUseCases.Queries;
using Bombers_System.Domain.DTOs.Vehicle;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bombers_System.Controllers;

[Authorize]
[ApiController]
[Route("api/vehicles")]
public class VehicleController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehicleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? type,
        [FromQuery] string? status,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(type) || !string.IsNullOrWhiteSpace(status))
        {
            var filtered = await _mediator.Send(new GetVehiclesFilteredQuery(type, status), cancellationToken);
            return Ok(filtered);
        }

        var result = await _mediator.Send(new GetAllVehiclesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetVehicleByIdQuery(id), cancellationToken);
        if (result is null) return NotFound(new { message = $"Vehículo con ID {id} no encontrado." });
        return Ok(result);
    }

    [HttpGet("station/{stationId:int}")]
    public async Task<IActionResult> GetByStation([FromRoute] int stationId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetVehiclesByStationQuery(stationId), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Create([FromBody] CreateVehicleDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _mediator.Send(new CreateVehicleCommand(dto), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.VehicleId }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateVehicleDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _mediator.Send(new UpdateVehicleCommand(id, dto), cancellationToken);
        if (result is null) return NotFound(new { message = $"Vehículo con ID {id} no encontrado." });
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var deleted = await _mediator.Send(new DeleteVehicleCommand(id), cancellationToken);
        if (!deleted) return NotFound(new { message = $"Vehículo con ID {id} no encontrado." });
        return NoContent();
    }
}
