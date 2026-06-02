using Bombers_System.Application.UseCases.StationUseCases.Commands;
using Bombers_System.Application.UseCases.StationUseCases.Queries;
using Bombers_System.Domain.DTOs.Station;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bombers_System.Controllers;

[ApiController]
[Route("api/stations")]
public class StationController : ControllerBase
{
    private readonly IMediator _mediator;

    public StationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllStationsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetStationByIdQuery(id), cancellationToken);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStationDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateStationCommand(dto), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.StationId }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStationDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateStationCommand(id, dto), cancellationToken);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var deleted = await _mediator.Send(new DeleteStationCommand(id), cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
