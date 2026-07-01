using Bombers_System.Application.UseCases.IncidentUseCases.Commands;
using Bombers_System.Application.UseCases.IncidentUseCases.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bombers_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IncidentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public IncidentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // 🔵 GET: api/incidents
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllIncidentsQuery());
        return Ok(result);
    }

    // 🔵 GET: api/incidents/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetIncidentByIdQuery
        {
            IncidentId = id
        });

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    // 🟢 POST: api/incidents
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateIncidentCommand command)
    {
        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result }, result);
    }

    // 🟡 PUT: api/incidents
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateIncidentCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result)
            return BadRequest("No se pudo actualizar el incidente");

        return Ok("Incidente actualizado correctamente");
    }

    // 🟣 PATCH: api/incidents/status
    [HttpPatch("status")]
    public async Task<IActionResult> ChangeStatus([FromBody] ChangeIncidentStatusCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result)
            return BadRequest("No se pudo cambiar el estado");

        return Ok("Estado actualizado correctamente");
    }

    // 🔴 DELETE: api/incidents/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteIncidentCommand
        {
            IncidentId = id
        });

        if (!result)
            return NotFound("Incidente no encontrado");

        return Ok("Incidente eliminado correctamente");
    }
}