using Bombers_System.Application.UseCases.FirefighterUseCases.Commands;
using Bombers_System.Application.UseCases.FirefighterUseCases.Queries;
using Bombers_System.Domain.DTOs.Firefighter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bombers_System.Controllers;

[ApiController]
[Route("api/firefighters")]
public class FirefightersController : ControllerBase
{
    private readonly IMediator _mediator;

    public FirefightersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllFirefightersQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetFirefighterByIdQuery(id), cancellationToken);
        if (result is null) return NotFound();
        return Ok(result);
    }

    // El endpoint especial solicitado para listar por estación
    [HttpGet("station/{stationId:int}")]
    public async Task<IActionResult> GetByStation([FromRoute] int stationId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetFirefightersByStationQuery(stationId), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFirefighterDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateFirefighterCommand(dto), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.FirefighterId }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateFirefighterDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateFirefighterCommand(id, dto), cancellationToken);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var deleted = await _mediator.Send(new DeleteFirefighterCommand(id), cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }
}