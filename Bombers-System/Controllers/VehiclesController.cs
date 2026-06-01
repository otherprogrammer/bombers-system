using Bombers_System.Application.UseCases.VehicleUseCases.Commands;
using Bombers_System.Application.UseCases.VehicleUseCases.Queries;
using Bombers_System.Domain.DTOs.Vehicle;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bombers_System.Controllers;

[ApiController]
[Route("api/vehicles")]
public class VehiclesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehiclesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllVehiclesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetVehicleByIdQuery(id), cancellationToken);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpGet("station/{stationId:int}")]
    public async Task<IActionResult> GetByStation([FromRoute] int stationId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetVehiclesByStationQuery(stationId), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVehicleDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateVehicleCommand(dto), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.VehicleId }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateVehicleDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateVehicleCommand(id, dto), cancellationToken);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var deleted = await _mediator.Send(new DeleteVehicleCommand(id), cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
