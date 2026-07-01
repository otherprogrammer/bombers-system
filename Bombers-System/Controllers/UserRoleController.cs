using Bombers_System.Application.UseCases.UserRoleUseCases.Commands;
using Bombers_System.Application.UseCases.UserRoleUseCases.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bombers_System.Controllers;

[Authorize]
[ApiController]
[Route("api/userroles")]
public class UserRoleController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public UserRoleController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("user/{userId}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> GetByUserId([FromRoute] int userId, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetRolesByUserIdQuery(userId), cancellationToken));
    }
    
    [HttpGet("role/{roleId}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> GetByRoleId([FromRoute] int roleId, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetUsersByRoleIdQuery(roleId), cancellationToken));
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Create(AssignRoleToUserCommand command, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetByUserId), new { controller = "UserRole", userId = response.UserId }, response);
    }

    [HttpDelete("user/{userId}/role/{roleId}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Delete([FromRoute] int userId, [FromRoute] int roleId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new RemoveRoleFromUserCommand(userId, roleId), cancellationToken);
        return NoContent();
    }
}