using Bombers_System.Application.UseCases.TestUseCases.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bombers_System.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    private readonly IMediator _mediator;

    public TestController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> Greeting([FromRoute] string name)
    {
        var result = await _mediator.Send(new GetGreetingQuery(name));
        return Ok(result);
    }
}