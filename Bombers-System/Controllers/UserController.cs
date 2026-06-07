using Bombers_System.Application.UseCases.UserUseCases.Commands;
using Bombers_System.Application.UseCases.UserUseCases.Queries;
using Bombers_System.Domain.DTOs.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bombers_System.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public  UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetUsersQuery(), cancellationToken));
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetUserByIdQuery(id), cancellationToken));
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = response.UserId }, response);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserDto dto, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new UpdateUserCommand(id, dto.Username, dto.FirefighterId), cancellationToken));
    }

    [HttpPatch("{id}/password")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> ChangePassword([FromRoute] int id, [FromBody] ChangePasswordDto dto, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new ChangeUserPasswordCommand(id, dto.Password), cancellationToken));
    }
    
    [HttpPatch("{id}/username")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> UpdateUsername([FromRoute] int id, [FromBody] UpdateUsernameDto dto, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new UpdateUsernameCommand(id, dto.Username), cancellationToken));
    }
    
    [HttpPatch("{id}/firefighter")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> UpdateFirefighter([FromRoute] int id, [FromBody] UpdateUserFirefighterDto dto, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new UpdateUserFirefighterCommand(id, dto.FirefighterId), cancellationToken));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteUserCommand(id), cancellationToken);
        return NoContent();
    }
}