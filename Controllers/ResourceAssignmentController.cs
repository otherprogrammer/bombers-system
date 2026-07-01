using Bombers_System.Application.UseCases.ResourceAssignmentUseCases.Commands;
using Bombers_System.Application.UseCases.ResourceAssignmentUseCases.Queries;
using Bombers_System.Domain.DTOs.ResourceAssignment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bombers_System.Controllers;

/// <summary>
/// Módulo de Asignación de Recursos: asignar/desasignar vehículos y bomberos a un incidente,
/// y listar los recursos (vehículos + tripulación) asignados a un incidente.
/// </summary>
[Authorize]
[ApiController]
[Route("api")]
public class ResourceAssignmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public ResourceAssignmentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // ---- Vehículos asignados a un incidente (despachos) ----

    [HttpGet("incidents/{incidentId:int}/dispatches")]
    public async Task<IActionResult> GetDispatchesByIncident([FromRoute] int incidentId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDispatchesByIncidentQuery(incidentId), cancellationToken);
        return Ok(result);
    }

    [HttpGet("incidents/{incidentId:int}/resources")]
    public async Task<IActionResult> GetIncidentResources([FromRoute] int incidentId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetIncidentResourcesQuery(incidentId), cancellationToken);
        return Ok(result);
    }

    [HttpPost("incidents/{incidentId:int}/dispatches")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> AssignVehicle([FromRoute] int incidentId, [FromBody] AssignVehicleDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _mediator.Send(new AssignVehicleCommand(incidentId, dto), cancellationToken);
        return CreatedAtAction(nameof(GetDispatchesByIncident), new { incidentId }, result);
    }

    [HttpPut("dispatches/{dispatchId:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> UpdateDispatch([FromRoute] int dispatchId, [FromBody] UpdateDispatchDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateDispatchCommand(dispatchId, dto), cancellationToken);
        if (result is null) return NotFound(new { message = $"Despacho con ID {dispatchId} no encontrado." });
        return Ok(result);
    }

    [HttpDelete("dispatches/{dispatchId:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> UnassignVehicle([FromRoute] int dispatchId, CancellationToken cancellationToken)
    {
        var deleted = await _mediator.Send(new UnassignVehicleCommand(dispatchId), cancellationToken);
        if (!deleted) return NotFound(new { message = $"Despacho con ID {dispatchId} no encontrado." });
        return NoContent();
    }

    // ---- Tripulación (bomberos) de un despacho ----

    [HttpGet("dispatches/{dispatchId:int}/crew")]
    public async Task<IActionResult> GetCrewByDispatch([FromRoute] int dispatchId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCrewByDispatchQuery(dispatchId), cancellationToken);
        return Ok(result);
    }

    [HttpPost("dispatches/{dispatchId:int}/crew")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> AssignCrewMember([FromRoute] int dispatchId, [FromBody] AssignCrewMemberDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _mediator.Send(new AssignCrewMemberCommand(dispatchId, dto), cancellationToken);
        return CreatedAtAction(nameof(GetCrewByDispatch), new { dispatchId }, result);
    }

    [HttpDelete("dispatches/{dispatchId:int}/crew/{crewId:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> UnassignCrewMember([FromRoute] int dispatchId, [FromRoute] int crewId, CancellationToken cancellationToken)
    {
        var deleted = await _mediator.Send(new UnassignCrewMemberCommand(crewId), cancellationToken);
        if (!deleted) return NotFound(new { message = $"Tripulante con ID {crewId} no encontrado en el despacho {dispatchId}." });
        return NoContent();
    }
}
